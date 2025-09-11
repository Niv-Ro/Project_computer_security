# Enterprise Security Authentication System 🔐
Production-grade authentication system implementing HMAC-SHA256 password hashing with MySQL-backed credential store for secure web applications.

### 🎯 Impact
Collaborated with team to develop secure authentication backend preventing SQL injection and XSS attacks, implementing **salted HMAC-SHA256 password hashing** with MySQL credential management for enterprise-level security.

### 🛠️ Tech Stack
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white) ![ASP.NET](https://img.shields.io/badge/ASP.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) ![MySQL](https://img.shields.io/badge/mysql-4479A1.svg?style=for-the-badge&logo=mysql&logoColor=white) ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

### 🎨 Key Features

#### Security Implementation
- Salted HMAC-SHA256 password hashing algorithm
- MySQL-backed credential store for user management
- Protection against SQL injection attacks
- XSS (Cross-Site Scripting) prevention
- Secure session management

#### Database Architecture
- MySQL database schema design
- Provisioned database setup script (webapp.sql)
- Optimized credential storage structure
- Secure connection configuration
- Prepared statements for query execution

#### Web Application Structure
- ASP.NET Core backend implementation
- Configuration management via web.config
- Entry point through default.aspx
- Middleware for request validation
- Error handling and logging

#### Collaborative Development
- Team collaboration on code reviews
- Feature development coordination
- Security best practices implementation
- Documentation and code standards
- Version control with Git

### In order to run the project on your computer use the following steps:
1. Open MySQL workbench
2. Use the SQL file in order to make the wanted schema.
4. Run the following query on new SQL tab:
   
create user 'admin'@'localhost'
identified by 'admin';
grant all privileges on *.* to 'admin'@'localhost' with grant option;
create user 'admin'@'%' identified by 'admin';
grant all privileges on *.* to 'admin'@'%' with grant option;
select user, host from mysql.user;

(Please wrap the dot (.) with * after grant all privileges on ... Make sure you do it for both lines )


4. The connection string with the right setup is already configured in the web.config file.
5. Run the project from the default.aspx file (It should run from there automatically).

https://github.com/user-attachments/assets/76b021b3-3a55-4497-905e-1aabc8018ae8
   
