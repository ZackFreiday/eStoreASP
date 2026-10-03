eStoreASP – Online Store Web Application
Project Documentation
1. Project Description

The project represents a web-based online store system developed using ASP.NET Core MVC. The application allows registered users to browse available products, manage a shopping cart, place orders, and track their personal order history. Additionally, the system includes an administrative role that enables product and order management.

The application follows the Model–View–Controller (MVC) architectural pattern, ensuring separation between business logic, user interface, and data access layers. The system uses Microsoft SQL Server LocalDB as the relational database management system and Entity Framework Core as the Object-Relational Mapping (ORM) framework.

The system supports two main roles: User and Administrator.

Registered users are able to:

Create an account and log in securely.
Browse products displayed in the store.
Add products to a shopping cart.
Modify quantities within the cart.
Remove products from the cart.
Complete the checkout process.
View their personal order history and order status.

The administrator has extended permissions and is able to:

Create new products.
Edit existing product information.
Delete products from the system.
View all orders placed by users.
Update order status from “Pending” to “Completed”.
Completed orders cannot be modified further, ensuring data consistency.

The shopping cart functionality is implemented using session storage. Items added to the cart are serialized into JSON format and stored temporarily during the user’s session. Upon checkout, the cart data is converted into a persistent order stored in the database. During this process, product stock is validated and reduced accordingly.

The system ensures that stock levels are correctly updated when orders are placed, preventing overselling. The application also implements role-based authorization using ASP.NET Core Identity.

2. User Manual

To use the system, the application must be started from Visual Studio. Once the project runs, the homepage loads automatically.

A new user can create an account by selecting the “Register” option and entering a valid email address and password. After registration, the user can log in using their credentials.

Once logged in, the user can navigate to the “Store” page to view available products. Each product displays its name, price, description, stock availability, and image (if provided). Products can be added to the shopping cart using the “Add” button.

The shopping cart can be accessed through the navigation bar. Inside the cart, the user can increase or decrease the quantity of each product or remove items entirely. The total price is calculated automatically. To place an order, the user selects the “Checkout” button.

After checkout, an order confirmation page is displayed, showing the order ID, date, total amount, and ordered items. The cart is then cleared automatically.

Users can access their order history through the “My Orders” section, where all previous orders and their current statuses are displayed.

For administrative access, the system seeds a predefined administrator account. After logging in as an administrator, additional options become available in the navigation bar. The administrator can manage products and view all user orders. Order statuses can be updated from Pending to Completed. Once marked as Completed, orders cannot be modified.

3. Database Description

The application uses Microsoft SQL Server LocalDB as the database system. The database is automatically generated and managed through Entity Framework Core migrations.

The database is named “eStoreASP”.

The main tables in the database include:

Products
This table stores product-related information such as name, price, description, available stock, and image URL. Each product has a unique primary key.

Orders
This table stores order information, including the user who placed the order, the order date, total amount, and current status. Each order has a unique primary key and is linked to a registered user.

OrderItems
This table stores individual items within an order. Each order item references an order and stores product-related data at the time of purchase, including product name, price, and quantity.

In addition to these tables, ASP.NET Identity automatically creates tables required for authentication and authorization, including:

AspNetUsers
AspNetRoles
AspNetUserRoles
AspNetUserClaims
AspNetUserLogins
AspNetUserTokens

The relationship between Orders and OrderItems is one-to-many. A single order can contain multiple order items. Orders are also linked to users through a foreign key relationship.

Entity Framework Core migrations are used to manage schema updates and track database versioning through the __EFMigrationsHistory table.

4. External Components Used

The project is built using the following technologies and external components:

ASP.NET Core MVC framework
Entity Framework Core
Microsoft SQL Server LocalDB
ASP.NET Core Identity
Razor View Engine
Bootstrap 5 for user interface styling
Visual Studio development environment
.NET 8.0 framework

ASP.NET Core Identity is used to handle authentication, role management, and secure password storage. Entity Framework Core serves as the ORM layer, enabling database operations without writing raw SQL queries.

Bootstrap is used to enhance the user interface and provide responsive design and styled components.