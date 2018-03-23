# CareerApplicationFromBack-Backend
-This project was built on Visual Studio 2017. it may need only visual studio 2017 to run it. 

-The Database is atttached in the Database.zip due to many diffrances in MSSQL Versions the database script and back-up copy of the database are in the databse.zip. the database goes under the name "CareerApplicationFormDB".
Link to the Database 
https://drive.google.com/file/d/1IHSn7F2UoBlAIOMznO1QdCiTf5aXMWxQ/view?usp=sharing

the  current conniction string is you may change it on your needs

    <add name="umbracoDbDSN" connectionString="Server=LocalHost\SQLEXPRESS;Database=CareerApplicationFormDB;Integrated Security=true" providerName="System.Data.SqlClient" />
 

-due to many using of "Magic string" in umbraco all of them are saved in the web.config to gain more controll over the project.

-Mailling Services credentials are in the web.config.

-The new two custom buttons to (sendmMail and export ro excel) are exposed to the admins only.
click here to see where the custom buttons are placed https://drive.google.com/file/d/1eU2pU7HbBIeNvEQfD4XlSJJniUJ8gLei/view?usp=sharing
 they are registered on application started event. in custom CustomSettings folder.

AdminstratorLogin:UserName :info@Dopravo.com Password:Admin12345678

UserLogin:UserName :hr@Dopravo.com Password:Admin12345678 
