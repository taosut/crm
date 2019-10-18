ARG NODE_IMAGE=node:12.10

FROM ${NODE_IMAGE} as node-build
WORKDIR /app

COPY src/Web/CRM.Portal.Web/yarn.lock .
COPY src/Web/CRM.Portal.Web/package.json .
RUN yarn install

COPY src/Web/CRM.Portal.Web .
RUN yarn run build

FROM node-build as publish

FROM nginx:perl as final
WORKDIR /app

RUN nginx -v
EXPOSE 80

COPY --from=publish /app/nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=publish /app/nginx/default.conf /etc/nginx/conf.d/default.conf
COPY --from=publish /app/build /app

CMD ["nginx", "-g", "daemon off;"]
