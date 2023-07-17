import React, { useEffect, useState } from "react";
import { useTheme } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableFooter from "@mui/material/TableFooter";
import TablePagination from "@mui/material/TablePagination";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import IconButton from "@mui/material/IconButton";
import Button from "@mui/material/Button";
import FirstPageIcon from "@mui/icons-material/FirstPage";
import KeyboardArrowLeft from "@mui/icons-material/KeyboardArrowLeft";
import KeyboardArrowRight from "@mui/icons-material/KeyboardArrowRight";
import LastPageIcon from "@mui/icons-material/LastPage";
import Title from "./Title";
import {
  getPermissions,
  modifyPermission,
  requestPermission,
} from "../requests";
import { PaginationParams, Permission, PermissionPost } from "../models";
import AddEdit from "./AddEdit";
import { TableHead } from "@mui/material";

interface TablePaginationActionsProps {
  count: number;
  page: number;
  rowsPerPage: number;
  onPageChange: (
    event: React.MouseEvent<HTMLButtonElement>,
    newPage: number
  ) => void;
}

function TablePaginationActions(props: TablePaginationActionsProps) {
  const theme = useTheme();
  const { count, page, rowsPerPage, onPageChange } = props;

  const handleFirstPageButtonClick = (
    event: React.MouseEvent<HTMLButtonElement>
  ) => {
    onPageChange(event, 0);
  };

  const handleBackButtonClick = (
    event: React.MouseEvent<HTMLButtonElement>
  ) => {
    onPageChange(event, page - 1);
  };

  const handleNextButtonClick = (
    event: React.MouseEvent<HTMLButtonElement>
  ) => {
    onPageChange(event, page + 1);
  };

  const handleLastPageButtonClick = (
    event: React.MouseEvent<HTMLButtonElement>
  ) => {
    onPageChange(event, Math.max(0, Math.ceil(count / rowsPerPage) - 1));
  };

  return (
    <Box sx={{ flexShrink: 0, ml: 2.5 }}>
      <IconButton
        onClick={handleFirstPageButtonClick}
        disabled={page === 0}
        aria-label="first page"
      >
        {theme.direction === "rtl" ? <LastPageIcon /> : <FirstPageIcon />}
      </IconButton>
      <IconButton
        onClick={handleBackButtonClick}
        disabled={page === 0}
        aria-label="previous page"
      >
        {theme.direction === "rtl" ? (
          <KeyboardArrowRight />
        ) : (
          <KeyboardArrowLeft />
        )}
      </IconButton>
      <IconButton
        onClick={handleNextButtonClick}
        disabled={page >= Math.ceil(count / rowsPerPage) - 1}
        aria-label="next page"
      >
        {theme.direction === "rtl" ? (
          <KeyboardArrowLeft />
        ) : (
          <KeyboardArrowRight />
        )}
      </IconButton>
      <IconButton
        onClick={handleLastPageButtonClick}
        disabled={page >= Math.ceil(count / rowsPerPage) - 1}
        aria-label="last page"
      >
        {theme.direction === "rtl" ? <FirstPageIcon /> : <LastPageIcon />}
      </IconButton>
    </Box>
  );
}

const emptyPermission: PermissionPost = {
  nombreEmpleado: "",
  apellidoEmpleado: "",
  tipo: 1,
};

const Permissions: React.FC = () => {
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(5);
  const [rows, setRows] = useState<Permission[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [reload, setReload] = useState(false);
  const [initialValues, setInitialValues] =
    useState<PermissionPost>(emptyPermission);

  useEffect(() => {
    const getData = async () => {
      const paginationParams: PaginationParams = {
        pageNumber: page,
        pageSize: rowsPerPage,
      };
      const pagedPermissions = await getPermissions(paginationParams);
      setRows(pagedPermissions.items);
      setTotalCount(pagedPermissions.totalCount);
      setTotalPages(pagedPermissions.totalPages);
    };
    getData();
  }, [page, rowsPerPage, reload]);

  const handleOpenModal = () => {
    setInitialValues(emptyPermission);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
  };

  const handleSavePermission = async (values: PermissionPost) => {
    if (values.id) {
      await modifyPermission(values);
    } else {
      await requestPermission(values);
    }
    setIsModalOpen(false);
    setReload((reload) => !reload);
  };

  const handleEditPermission = async (permission: Permission) => {
    console.log("editando", permission.id);
    const permissionPost: PermissionPost = {
      id: permission.id,
      nombreEmpleado: permission.nombreEmpleado,
      apellidoEmpleado: permission.apellidoEmpleado,
      tipo: permission.tipo?.id ?? 1,
    };
    setInitialValues(permissionPost);
    setIsModalOpen(true);
  };

  const emptyRows =
    page > 0 ? Math.max(0, (1 + page) * rowsPerPage - rows.length) : 0;

  const handleChangePage = (
    event: React.MouseEvent<HTMLButtonElement> | null,
    newPage: number
  ) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  return (
    <>
      <TableContainer component={Paper}>
        <Title>Empleados</Title>
        <Table sx={{ minWidth: 500 }} aria-label="custom pagination table">
          <TableHead>
            <TableRow>
              <TableCell component="th" scope="row">
                <strong>Nombre empleado</strong>
              </TableCell>
              <TableCell
                style={{ width: 160 }}
                component="th"
                scope="row"
                align="right"
              >
                <strong>Apellido empleado</strong>
              </TableCell>
              <TableCell
                style={{ width: 160 }}
                component="th"
                scope="row"
                align="right"
              >
                <strong>Tipo permiso</strong>
              </TableCell>
              <TableCell
                style={{ width: 160 }}
                component="th"
                scope="row"
                align="right"
              >
                <strong>Fecha permiso</strong>
              </TableCell>
              <TableCell
                style={{ width: 160 }}
                component="th"
                scope="row"
              ></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {rows.map((row, index) => (
              <TableRow key={index}>
                <TableCell>{row.nombreEmpleado}</TableCell>
                <TableCell style={{ width: 160 }} align="right">
                  {row.apellidoEmpleado}
                </TableCell>
                <TableCell style={{ width: 160 }} align="right">
                  {row.tipo.description}
                </TableCell>
                <TableCell style={{ width: 160 }} align="right">
                  {new Date(row.fechaPermiso).toLocaleDateString()}
                </TableCell>
                <TableCell style={{ width: 100 }}>
                  <Button
                    variant="contained"
                    color="secondary"
                    onClick={() => handleEditPermission(row)}
                  >
                    Editar
                  </Button>
                </TableCell>
              </TableRow>
            ))}
            {emptyRows > 0 && (
              <TableRow style={{ height: 53 * emptyRows }}>
                <TableCell colSpan={5} />
              </TableRow>
            )}
          </TableBody>
          <TableFooter>
            <TableRow>
              <TablePagination
                rowsPerPageOptions={[5, 10, 25, 50]}
                colSpan={3}
                count={totalCount}
                rowsPerPage={rowsPerPage}
                page={page}
                SelectProps={{
                  inputProps: {
                    "aria-label": "rows per page",
                  },
                  native: true,
                }}
                onPageChange={handleChangePage}
                onRowsPerPageChange={handleChangeRowsPerPage}
                ActionsComponent={TablePaginationActions}
              />
            </TableRow>
          </TableFooter>
        </Table>
      </TableContainer>
      <Button variant="contained" color="primary" onClick={handleOpenModal}>
        Agregar Permiso
      </Button>

      <AddEdit
        initialValues={initialValues}
        onSubmit={handleSavePermission}
        isModalOpen={isModalOpen}
        closeModal={handleCloseModal}
      />
    </>
  );
};
export default Permissions;
