FROM node:lts AS development

WORKDIR /frontend

# add `/ui/node_modules/.bin` to $PATH
ENV PATH /frontend/node_modules/.bin:$PATH

# install app dependencies
COPY package.json ./

RUN npm install

# add app
COPY . .

# open app port
EXPOSE 3000

# start app
CMD ["npm", "start"]