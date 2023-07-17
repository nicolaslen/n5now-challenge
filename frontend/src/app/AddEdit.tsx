import React, { useEffect, useState } from "react";
import { Formik, Form, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import { Permission, PermissionPost, PermissionType } from "../models";
import { Box, Button, Modal, Typography } from "@mui/material";
import { getTypes } from "../requests";

interface AddEditProps {
  initialValues: PermissionPost;
  onSubmit: (values: PermissionPost) => Promise<void>;
  isModalOpen: boolean;
  closeModal: () => void;
}

const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

const AddEdit: React.FC<AddEditProps> = ({
  isModalOpen,
  initialValues,
  onSubmit,
  closeModal,
}) => {
  const [types, setTypes] = useState<PermissionType[]>([]);

  useEffect(() => {
    const getData = async () => {
      const types = await getTypes();
      setTypes(types);
    };
    getData();
  }, []);

  const validationSchema = Yup.object().shape({
    id: Yup.number(),
    nombreEmpleado: Yup.string().required("Nombre de empleado requerido"),
    apellidoEmpleado: Yup.string().required("Apellido de empleado requerido"),
    tipo: Yup.number().required("Tipo de permiso requerido"),
  });

  const handleSubmit = async (
    values: PermissionPost,
    { setSubmitting }: any
  ) => {
    if (initialValues.id) {
      values.id = initialValues.id;
    }
    console.log(values);
    await onSubmit(values);
    setSubmitting(false);
  };

  return (
    <Modal open={isModalOpen}>
      <Box sx={style}>
        <Typography id="modal-modal-title" variant="h6" component="h2">
          {initialValues.id ? "Modificar permiso" : "Agregar permiso"}
        </Typography>
        <Formik
          initialValues={initialValues}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}
        >
          <Form>
            <div>
              <label htmlFor="nombreEmpleado">Nombre de empleado</label>
              <Field type="text" id="nombreEmpleado" name="nombreEmpleado" />
              <ErrorMessage
                name="nombreEmpleado"
                component="div"
                className="error"
              />
            </div>

            <div>
              <label htmlFor="apellidoEmpleado">Apellido de empleado</label>
              <Field
                type="text"
                id="apellidoEmpleado"
                name="apellidoEmpleado"
              />
              <ErrorMessage
                name="apellidoEmpleado"
                component="div"
                className="error"
              />
            </div>

            <div>
              <label htmlFor="tipo">Tipo de permiso</label>
              <Field as="select" id="tipo" name="tipo">
                {types.map((type, index) => (
                  <option key={index} value={type.id}>
                    {type.description}
                  </option>
                ))}
              </Field>
              <ErrorMessage name="tipo" component="div" className="error" />
            </div>

            <Button type="submit" variant="contained" color="primary">
              Guardar
            </Button>
            <Button onClick={closeModal} variant="contained" color="secondary">
              Cancelar
            </Button>
          </Form>
        </Formik>
      </Box>
    </Modal>
  );
};

export default AddEdit;
