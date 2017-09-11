# Module 2: Querying and Manipulating Data Using Entity Framework

# Lesson 2: Creating an Entity Data Model

### Demonstration: Creating Classes and Database By Using Code-first

#### Preparation Steps

TODO: Virtual machines are irrelevant, anything else instead?

#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **New** , and then click **Project**.
3. In the **New Project** dialog box, using the navigation pane, expand the **Installed** node, expand the **Templates** node, and select the **Visual C#** node. From the list of templates, select **Console App**.
4. In the **Name** box, type **MyFirstEF**.
5. In the **Location** box, type **D:\Allfiles\Mod02\Democode**
6. Select the **Create directory for solution** check box, and then click **OK**.
7. In Solution Explorer, right-click the **MyFirstEF** project, and then click **Manage NuGet Packages**. 
8. In the **NuGet: MyFirstEF** tab, select the **Browse** node on the left pane.
9. On the upper left side of the dialog box, click the **Search** box, and type **EntityFramework**.
10. In the search results, select **EntityFramework** , and then click **Install**. If a **Preview** dialog box appears, click **Ok**. If a **License Acceptance** dialog box appears, click **I Accept**.
11. Wait until the package is completely downloaded and installed. Close the **NuGet: MyFirstEF** tab.
12. In Solution Explorer, right-click the **MyFirstEF** project, point to **Add** , and then click **Class**.
13. In the **Add New Item** dialog box, type **Product** in the **Name** box, and then click **Add**.
14. In **Product.cs** , change the class definition to the match the following code.

```cs
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```
15. To save the changes, press Ctrl+S.
16. In Solution Explorer, right-click the **MyFirstEF** project, point to **Add** , and then click **Class**.
17. In the **Add New Item** dialog box, type **Store** in the **Name** box, and then click **Add**.
18. In **Store.cs** , change the class definition to match the following code.

```cs
public class Store
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Product> Products { get; set; }
}
```
19. To save the changes, press Ctrl+S.
20. In Solution Explorer, right-click the **MyFirstEF** project, point to **Add** , and then click **Class**.
21. In the **Add New Item** dialog box, type **MyDbContext** in the **Name** box, and then click **Add**.
22. In **MyDbContext.cs** , add the following **using** directive to the beginning of the file.

```cs
using System.Data.Entity;
```
23. Change the class definition to match the following.

```cs
public class MyDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }
}
```
24. To save the changes, press Ctrl+S.
25. In Solution Explorer, under the **MyFirstEF** project, double-click **Program.cs** , and add the following **using** directive to the beginning of the file.

```cs
using System.Data.Entity;
```
26. Add the following code to the **Main** method.

```cs
var dbInitializer = new CreateDatabaseIfNotExists<MyDbContext>();
dbInitializer.InitializeDatabase(new MyDbContext());
```
27. To save the changes, press Ctrl+S.
28. To run the application, press F5.
29. The application now creates a new database on the local SQL Express, named **MyFirstEF.MyDbContext.**
30. On the Start screen, expand **Microsoft SQL Server 2014** and then click the **SQL Server 2014 Management Studio** tile.
31. In the **Server Name** box, type **.\SQLEXPRESS**.
32. In the **Authentication** drop-down menu, verify **Windows Authentication** is selected, and then click **Connect**.
33. In Object Explorer, expand the **Databases** node.
34. Make sure you see a database named **MyFirstEF.MyDbContext**.
35. In Object Explorer, expand the **MyFirstEF.MyDbContext** node, and then expand the **Tables** node.
36. Notice that both classes defined in the **MyFirstEF** project appear as tables, **dbo.Stores** and **dbo.Products**.

>**Note** : Database tables are usually named in the plural form, which is why Entity Framework changed the names of the generated tables from Store and Product to Stores and Products. The _dbo_ prefix is the name of the schema in which the tables were created.

37. Expand **dbo.Products** and **dbo.Stores** tables, and then expand the **Columns** node in each of them to see that both tables have **Id** and **Name** columns, similar to their corresponding class properties.
38. Close SQL Server Management Studio.





# Lesson 3: Querying Data

### Demonstration 1: Using LINQ to Entities

#### Preparation Steps

TODO: What do we do instead of VMs, if at all?

#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod02\Democode\UsingLINQtoEntities\Begin**.
4. Click **EF\_CodeFirst.sln** , and then click **Open**.
5. In Solution Explorer, under the **EF\_CodeFirst** project, double-click **Program.cs**.
6. Create a new **SchoolContext** object by appending the following code to the **Main** method, after the **InitializeDatabase**   method call.

```cs
using (var context = new SchoolContext())
{
}
```
7. You use the **using** statement to control the release of unmanaged resources used by the context, such as a database connection.
8. To select all courses from the database, add the following LINQ to Entities code in the **using** block

```cs
var courses = from c in context.Courses
select c;
```
9. To print courses and students list to the console window, add the following code in the **using** block after the LINQ to Entities code.

```cs
foreach (var course in courses)
{
	Console.WriteLine($"Course: {course.Name})";
	foreach (var student in course.Students)
	{
		Console.WriteLine($"\tStudent name: {student.Name}");
	}
}
Console.ReadLine();
```
10. Press Ctrl+S to save the changes.
11. In the **Main** method, next to the **Console.ReadLine()** line number, left click on the left-hand side bar, before the line number to **insert Breakpoint**.
12. To run the application, press F5. After a few seconds, the code execution will break, and the breakpoint will be highlighted in yellow.
13. In the console window, review the course and student lists printed to the console window.
14. In Visual Studio 2017, on the **Debug** menu, point to **IntelliTrace** , and then click **IntelliTrace Events**.
15. Review the SQL statements executed by Entity Framework. The first set of queries is part of the database initializer code. The next set of queries is a single query to get the list of courses, and another set of queries to get the list of students, one query for each course.

>**Note** : IntelliTrace will be covered in Module 10, &quot;Monitoring and Diagnostics&quot; in Course 20487.

16. To stop the debugger, press Shift+F5.

### Demonstration 2: Running Stored Procedures with Entity Framework

#### Preparation Steps

For this demo, you will use the available virtual machine environment. Before you begin this demo, you must complete the following steps:  
1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.  
2. In Hyper-V Manager, click **MSL-TMG1**, and in the Action pane, click **Start**.  
3. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the Action pane, click **Start**.  
4. In the Action pane, click **Connect**. Wait until the virtual machine starts.  
5. Logon using the following credentials:  
- User name: **Administrator**
- Password: **Pa$$w0rd**

#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2012** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Go to **D:\Allfiles\Mod02\Democode\StoredProcedure\Begin**.
4. Click **EF\_CodeFirst.sln** , and then click **Open**.
5. In Solution Explorer, expand the **EF\_CodeFirst** project, and double-click the **Program.cs** file.
6. Go to the **Main** method, and notice that a **SchoolContext** instance is created to establish a connection to the database.
7. Review the query that is being assigned to the **averageGradeInCourse** variable and notice that the average grade of the WCF course is calculated, and then printed to the console.
8. The **ExecuteSqlCommand** statement calls a stored procedure named **spUpdateGrades** with two parameters, **CourseName** and **GradeChange**.
9. To run the console application, press Ctrl+F5. Notice that the updated average grade is printed to the console before and after the change.




# Lesson 4: Manipulating Data

### Demonstration: CRUD Operations in Entity Framework

#### Preparation Steps

For this demo, you will use the available virtual machine environment. Before you begin this demo, you must complete the following steps:  
1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.  
2. In Hyper-V Manager, click **MSL-TMG1**, and in the Action pane, click **Start**.  
3. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the Action pane, click **Start**.  
4. In the Action pane, click **Connect**. Wait until the virtual machine starts.  
5. Logon using the following credentials:  
- User name: **Administrator**  
- Password: **Pa$$w0rd**

#### Demonstration Steps

1.  On the Start screen, click the Visual Studio 2012 tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod02\Democode\CRUD\Begin**.
4. Click **EF\_CodeFirst.sln** , and then click **Open**.
5. In Solution Explorer, under the **EF\_CodeFirst** project, double-click **Program.cs**.
6. Create a new **SchoolContext** object by appending the following code to the **Main** method, after the **InitializeDatabase**  method call.

	```cs
using (var context = new SchoolContext())
{
}
```
7. Query the **WCF** course by appending the following code inside the **using** block.

```cs
Course WCFCourse = (from course in context.Courses 
		where course.Name == "WCF" select course).Single();
```
8. To create two new students named **Thomas Anderson** and **Terry Adams** , append the following code to the **using** block.

```cs
Student firstStudent = new Student() { Name = "Thomas Anderson" };
Student secondStudent = new Student() { Name = "Terry Adams" };
```
9. To add the two newly created students to the **WCF** course, append the following code to the **using** block.

```cs
WCFCourse.Students.Add(firstStudent);
WCFCourse.Students.Add(secondStudent);
```
10. To give the teacher of the **WCF** course a $1000 salary raise, append the following code to the **using** block.

```cs
WCFCourse.CourseTeacher.Salary += 1000;
```
11. To select a student named **Student_1** from the **WCF** course, append the following code to the **using** block.

```cs
Student studentToRemove = WCFCourse.Students.Where((student) => student.Name == "Student_1").FirstOrDefault();
```
12. To remove the student from the **WCF** course, append the following code to the **using** block.

```cs
WCFCourse.Students.Remove(studentToRemove);
```
13. To save the changes and print the result, append the following code to the **using** block.

```cs
context.SaveChanges();
Console.WriteLine(WCFCourse);
Console.ReadLine();
```
14. Press Ctrl+S to save the changes.
15. In the **Main** method, right-click the _Console.ReadLine()_ method call, point to **Breakpoint** , and then click **Insert Breakpoint**.
16. To run the application, press F5. After a few seconds, the code execution will break, and the breakpoint will be highlighted in yellow.
17. The list of students appears in the console window. Notice that there are two new students at the bottom of the list, and student 1 is missing from the list. Also notice that the salary of the teacher is now 101000.
18. In Visual Studio 2012, on the **Debug** menu, point to **IntelliTrace** , and then click **IntelliTrace Events**.
19. Notice the SQL update, delete, and insert statements that correspond to the salary update, student deletion, and the addition of the two new students.
20. To stop the debugger, press Shift+F5.

©2016 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.
