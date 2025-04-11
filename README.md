# BU.OnlineShop

## What is the Application

BU.OnlineShop is a microservices-based online shopping platform designed to provide a comprehensive shopping experience. It includes various services such as:

- **Ordering Service**: Manages customer orders and order processing.
- **Catalog Service**: Handles product catalog management.
- **Basket Service**: Manages customer shopping baskets.
- **File Service**: Manages file handling and storage.
- **Web Gateway**: Provides a unified interface for user interaction with the platform.

Each service is designed to be independently deployable and scalable, leveraging technologies such as ASP.NET Core, Entity Framework Core, MassTransit for messaging, and Keycloak for identity management, authentication and authorization.

Keycloak is used to manage authentication and authorization across all services in the BU.OnlineShop platform. It provides a centralized identity management solution, ensuring secure access to resources and protecting sensitive data. Each service in the BU.OnlineShop platform is configured to use Keycloak for authentication and authorization.

## Requirements

- .NET 8.0 SDK
- Docker


## How to Run Locally

1. Run `docker-infrastructure-run.ps1` script to run Keycloak, RabbitMQ, and Aspire Dashboard.

2. Login to keycloak with username: admin and password: admin. Import the `realm.json` file into Keycloak to set up the necessary realm, clients, and roles.
   You should see `onlineshop` realm. 

3. Create users in the Keycloak admin console, assigning them appropriate roles and permissions.

4. Navigate to clients => OnlineShop_Swagger => Credentials tab => Regenerate client secret.

5. You should change the Keycloak secret configuration in `appsettings.json` to the following for each service:
    ```json
    {
      "Keycloak": {
        "Credentials": {
          "Secret": "your_client_secret"
        }
      }
    }
    ```
    ```json
    {
      "Swagger": {
        "ClientSecret": "your_client_secret"
        },
    }
    ```
    Besides that, you can use `appsettings.json` defaultly. If you have different setup, you should update it accordingly.

6. Navigate to the root directory of the project:
    ```sh
    cd BU.OnlineShop
    ```

7. Build the solution:
    ```sh
    dotnet build
    ```
8. Update the database with the new migration:
    ```sh
    dotnet ef database update
    ```

    Repeat the above steps for each of the following services:
      - `services/basket/BU.OnlineShop.BasketService.API`
      - `services/catalog/BU.OnlineShop.CatalogService.API`
      - `services/file/BU.OnlineShop.FileService.API`
      - `services/order/BU.OnlineShop.OrderingService.API`

    Make sure that the connection strings in the `appsettings.json` files are correctly configured for each service before applying migrations.


9. Navigate to the directory of each service and run the service:
    ```sh
    cd services/order/BU.OnlineShop.OrderingService.API
    dotnet run
    ```

    Repeat the above steps for each of the following services:
    - `services/payment/BU.OnlineShop.PaymentService.API`
    - `services/basket/BU.OnlineShop.BasketService.API`
    - `services/catalog/BU.OnlineShop.CatalogService.API`
    - `services/file/BU.OnlineShop.FileService.API`
    - `services/order/BU.OnlineShop.OrderingService.API`
    - `gateways/BU.OnlineShop.WebGateway`

10. Access the services via the URLs specified in their respective `launchSettings.json` files.

## How to Run on Docker Environment

1. If you've run `docker-infrastructure-run.ps1` script, you should remove the Keycloak container. 

2. You should uncomment the environment variable in keycloak section in `docker-compose.infrastructure.override.yml`
```yml
  keycloak:
    ports:
      - 8080:8080
    environment:
      - KC_HOSTNAME=onlineshop-keycloak.localhost
```
3. You should set `onlineshop-keycloak.localhost` dns record in `C:\Windows\System32\drivers\etc\hosts` file.

4. Run `docker-infrastructure-run.ps1` script to run Keycloak, RabbitMQ, and Aspire Dashboard. If you've run `docker-infrastructure-run.ps1` before and set up the application running locally, you can skip 5, 6, and 7.

5. Login to keycloak with username: admin and password: admin. Import the `realm.json` file into Keycloak to set up the necessary realm, clients, and roles.
   You should see `onlineshop` realm. 

6. Create users in the Keycloak admin console, assigning them appropriate roles and permissions.

7. Navigate to clients => OnlineShop_Swagger => Credentials tab => Regenerate client secret.

8. You should change the Keycloak secret configuration in `docker-compose.override.yml` to the following for each service:
    ```yml
        orderservice:
          environment:
           - Keycloak__Credentials__Secret=your_client_secret
           - Swagger__ClientSecret=your_client_secret
    ```
    Besides that, you can use `docker-compose.override.yml` defaultly. If you have different setup, you should update it accordingly.

9. Run `docker-run.ps1` script

10. Access the services via the URLs specified in their respective `launchSettings.json` files.

## How to Run on Kubernetes

1. Ensure you have a Kubernetes cluster running and `kubectl` is configured to interact with it.

2. Ensure you have Nginx Ingress in your kubernetes cluster.

3. You should set dns records in `C:\Windows\System32\drivers\etc\hosts` file.
    - 127.0.0.1 basketservice.onlineshop.com
    - 127.0.0.1 catalogservice.onlineshop.com
    - 127.0.0.1 fileservice.onlineshop.com
    - 127.0.0.1 orderingservice.onlineshop.com
    - 127.0.0.1 paymentservice.onlineshop.com
    - 127.0.0.1 webgateway.onlineshop.com
    - 127.0.0.1 keycloak.onlineshop.com
    - 127.0.0.1 aspire.onlineshop.com

4. Navigate to the `k8s` directory:
    ```sh
    cd BU.OnlineShop/k8s
    ```

5. Apply the Kubernetes manifests:
    ```sh
    kubectl apply -f .
    ```

6. Verify that all services are running:
    ```sh
    kubectl get pods
    ```

7. Access the services via the URLs specified in section 2. Since there is no real certificate generated, you should trust all services in your browser.

8. Login to keycloak with username: admin and password: admin. Import the `realm.json` file into Keycloak to set up the necessary realm, clients, and roles.
   You should see `onlineshop` realm. 

9. Create users in the Keycloak admin console, assigning them appropriate roles and permissions.

10. Navigate to clients => OnlineShop_Swagger => Credentials tab => Regenerate client secret.

11. You should change the Keycloak secret configuration in environment section to the following for each service and apply deployment files.
    ```yml
      containers:
        - name: basketservice
          image: btrlpuyr/basketservice:latest
          env:
            - name: Keycloak__Credentials__Secret
              value: "qTQmyjpJSWQPSYXYZEQZQ0V9X0oh4p6c"
            - name: Swagger__ClientSecret
              value: "qTQmyjpJSWQPSYXYZEQZQ0V9X0oh4p6c"
    ```

12. You can navigate to `webgateway.onlineshop.com` in your browser to see all services or you can visit them separately.