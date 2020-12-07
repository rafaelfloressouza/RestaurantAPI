-- TOTAL OF 124 STORED PROCEDURES
-- USER STORED PROCEDURES

CREATE FUNCTION "spUser_GetAll"() RETURNS SETOF "User" AS 'SELECT * FROM "User";' LANGUAGE 'sql';

CREATE FUNCTION "spUser_GetById"(id integer) RETURNS "User" AS 'SELECT * FROM "User" WHERE id="ID";' LANGUAGE 'sql';

CREATE FUNCTION "spUser_InsertValue"(username VARCHAR, password VARCHAR,  firstname VARCHAR, middlename VARCHAR, lastname VARCHAR,  givenname VARCHAR, addr1 VARCHAR,  addr2 VARCHAR, province VARCHAR, postalcode VARCHAR, sex VARCHAR, phone VARCHAR, dob TIMESTAMP, email VARCHAR) RETURNS void AS $$ INSERT INTO "User"("Username", "Password", "FirstName", "MiddleName", "LastName", "GivenName", "Addr1", "Addr2", "Province", "PostalCode", "Sex", "Phone", "DOB", "Email") VALUES (username, password, firstname, middlename, lastname,  givenname, addr1, addr2, province, postalcode, sex, phone, dob, email); $$ LANGUAGE 'sql';

CREATE FUNCTION "spUser_ModifyById"(id int, username VARCHAR, password VARCHAR, firstname VARCHAR, middlename VARCHAR, lastname VARCHAR, givenname VARCHAR, addr1 VARCHAR, addr2 VARCHAR, province VARCHAR, postalcode VARCHAR, sex VARCHAR, phone VARCHAR, dob TIMESTAMP, email VARCHAR) RETURNS void AS $$ UPDATE "User" SET "Username"=username, "Password"=password, "FirstName"=firstname, "MiddleName"=middlename, "LastName"=lastname, "GivenName"=givenname, "Addr1"=addr1, "Addr2"=addr2, "Province"=province,
"PostalCode"=postalcode, "Sex"=sex, "Phone"=phone, "DOB" = dob, "Email" = email WHERE id="ID";$$ LANGUAGE 'sql';

CREATE FUNCTION "spUser_DeleteById"(id int) RETURNS void AS 'DELETE FROM "User" WHERE id="ID";' LANGUAGE 'sql';

CREATE FUNCTION "spUser_LastInserted"(out cur_user_id int) AS  $$ SELECT "ID" FROM "User" ORDER BY "ID" DESC LIMIT 1; $$ LANGUAGE 'sql';

-- CUSTOMER STORED PROCEDURES

CREATE FUNCTION "spCustomer_GetAll"() RETURNS SETOF "Customer" AS 'SELECT * FROM "Customer";' LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_GetById"(id integer) RETURNS "Customer" AS 'SELECT * FROM "Customer" WHERE id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_InsertValue"(user_id int, table_no int) RETURNS void AS 'INSERT INTO "Customer"("User_ID","TableNo") VALUES (user_id, table_no);' LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_Sit"(user_id int, tableno int) RETURNS void AS 'UPDATE "Customer" SET "TableNo" = tableno WHERE user_id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_Leave"(user_id int) RETURNS void AS 'UPDATE "Customer" SET "TableNo" = null WHERE user_id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_DeleteById"(id int) RETURNS void AS 'DELETE FROM "Customer" WHERE id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_AtTable"(tableno int) RETURNS SETOF "Customer" AS 'SELECT * FROM "Customer"WHERE "TableNo"=tableno;' LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_GetTransactions"(user_id int) RETURNS SETOF "Transaction" AS $$ SELECT t."Transaction_ID", t."Amount", t."Date_Time" FROM "Customer_Transaction" AS ct, "Transaction" AS t WHERE ct."User_ID"=user_id AND ct."Transaction_ID"=t."Transaction_ID"; $$ LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_GetReviews"(user_id int) RETURNS SETOF "Review" AS $$ SELECT r."User_ID", r."Review_ID", r."Description", r."Rating", r."Dish_ID" FROM "Review" AS r WHERE r."User_ID"=user_id; $$ LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_GetOrders"(user_id int) RETURNS SETOF "Order" AS $$ SELECT * FROM "Order" AS o WHERE o."User_ID"=user_id; $$ LANGUAGE 'sql';

-- DISH STORED PROCEDURES

CREATE FUNCTION "spDish_GetAll"() RETURNS SETOF "Dish" AS 'SELECT * FROM "Dish";' LANGUAGE 'sql';

CREATE FUNCTION "spDish_GetById"(id integer) RETURNS "Dish" AS 'SELECT * FROM "Dish" WHERE id="Dish_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spDish_InsertValue"(dish_id int, available boolean, price decimal, description varchar, menu_type varchar) RETURNS void AS  'INSERT INTO "Dish"("Dish_ID", "Available", "Price", "Description", "Menu_Type") VALUES (dish_id, available, price, description, menu_type);' LANGUAGE 'sql';

CREATE FUNCTION "spDish_ModifyById"(id int, available boolean, price decimal, description varchar, menu_type varchar) RETURNS void AS 'UPDATE "Dish" SET "Available" = available,"Price" = price,"Description" = description, "Menu_Type" = menu_type WHERE id="Dish_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spDish_DeleteById"(id int) RETURNS void AS 'DELETE FROM "Dish" WHERE id="Dish_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spDish_GetIngredients"(dish_id int) RETURNS TABLE(Name varchar) AS $$ 
SELECT i."Name" FROM "Dish_Ingredient" AS di, "Ingredient" AS i WHERE di."Dish_ID"=dish_id AND di."Ing_Name"=i."Name"; $$ LANGUAGE 'sql';

CREATE FUNCTION "spDish_GetAvailable"() RETURNS SETOF "Dish" AS $$ SELECT * FROM "Dish" WHERE "Available"=true; $$ LANGUAGE 'sql';

CREATE FUNCTION "spDish_MakeAvailable"(dish_id int)RETURNS void AS $$ UPDATE "Dish" SET "Available" = true WHERE dish_id="Dish_ID"; $$ LANGUAGE 'sql';

CREATE FUNCTION "spDish_MakeUnavailable"(dish_id int)RETURNS void AS $$ UPDATE "Dish" SET "Available" = false WHERE dish_id="Dish_ID"; $$ LANGUAGE 'sql';

CREATE FUNCTION "spDish_LastInserted"(out cur_dish_id int) AS  $$ SELECT "Dish_ID" FROM "Dish" ORDER BY "Dish_ID" DESC LIMIT 1; $$ LANGUAGE 'sql';

-- COOK STORED PROCEDURES

CREATE FUNCTION "spCook_GetAll"() RETURNS SETOF "Cook" AS 'SELECT * FROM "Cook";' LANGUAGE 'sql';

CREATE FUNCTION "spCook_GetById"(id integer) RETURNS "Cook" AS 'SELECT * FROM "Cook" WHERE id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spCook_InsertValue"(user_id int, specialty varchar, type varchar) RETURNS void AS 'INSERT INTO "Cook"("User_ID", "Specialty", "Type") VALUES (user_id, specialty, type);' LANGUAGE 'sql';

CREATE FUNCTION "spCook_ModifyById"(user_id int, specialty varchar, type varchar)RETURNS void AS 'UPDATE "Cook" SET "Specialty" = specialty,"Type" = type WHERE user_id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spCook_DeleteById"(user_id int) RETURNS void AS 'DELETE FROM "Cook" WHERE user_id="User_ID";' LANGUAGE 'sql';

-- EMPLOYEE STORED PROCEDURES

CREATE FUNCTION "spEmployee_GetAll"() RETURNS SETOF "Employee" AS 'SELECT * FROM "Employee";' LANGUAGE 'sql';

CREATE FUNCTION "spEmployee_GetById"(id integer) RETURNS "Employee" AS 'SELECT * FROM "Employee" WHERE id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spEmployee_InsertValue"(user_id int, start_date timestamp, job_title varchar, salary numeric, mgr_id int) RETURNS void AS 'INSERT INTO "Employee"("User_ID", "Start_Date", "Job_Title", "Salary", "mgr_ID") VALUES (user_id, start_date, job_title, salary, mgr_id);' LANGUAGE 'sql';

CREATE FUNCTION "spEmployee_ModifyById"(user_id int, start_date timestamp, job_title varchar, salary numeric, mgr_id int)RETURNS void AS 'UPDATE "Employee" SET "Start_Date" = start_date,"Job_Title" = job_title, "Salary" = salary, "mgr_ID" = mgr_id WHERE user_id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spEmployee_DeleteById"(user_id int) RETURNS void AS 'DELETE FROM "Employee" WHERE user_id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spEmployee_GetManagedBy"(manager_id integer) RETURNS SETOF "Employee" AS $$ SELECT * FROM "Employee" AS e WHERE e."mgr_ID"=manager_id; $$ LANGUAGE 'sql';

-- MANAGER STORED PROCEDURES

CREATE FUNCTION "spManager_GetAll"() RETURNS SETOF "Manager" AS 'SELECT * FROM "Manager";' LANGUAGE 'sql';

CREATE FUNCTION "spManager_GetById"(user_id integer) RETURNS "Manager" AS 'SELECT * FROM "Manager" WHERE user_id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spManager_InsertValue"(user_id int, area varchar) RETURNS void AS 'INSERT INTO "Manager"("User_ID", "Area") VALUES (user_id, area);' LANGUAGE 'sql';

CREATE FUNCTION "spManager_ModifyById"(user_id int, area varchar)RETURNS void AS 'UPDATE "Manager" SET "Area" = area WHERE user_id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spManager_DeleteById"(user_id int) RETURNS void AS 'DELETE FROM "Manager" WHERE user_id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spManager_GetNumberManagers"(out num_of_managers bigint) AS  $$ SELECT COUNT(*) FROM "Manager"; $$ LANGUAGE 'sql';

-- WAITER STORED PROCEDURES

CREATE FUNCTION "spWaiter_GetAll"() RETURNS SETOF "Waiter" AS 'SELECT * FROM "Waiter";' LANGUAGE 'sql';

CREATE FUNCTION "spWaiter_GetById"(user_id integer) RETURNS "Waiter" AS 'SELECT * FROM "Waiter" WHERE user_id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spWaiter_InsertValue"(user_id int, hours numeric, type varchar) RETURNS void AS 'INSERT INTO "Waiter"("User_ID", "Hours", "Type") VALUES (user_id, hours, type);' LANGUAGE 'sql';

CREATE FUNCTION "spWaiter_ModifyById"(user_id int, hours numeric, type varchar)RETURNS void AS 'UPDATE "Waiter" SET "Hours" = hours, "Type" = type WHERE user_id="User_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spWaiter_DeleteById"(user_id int) RETURNS void AS 'DELETE FROM "Waiter" WHERE user_id="User_ID";' LANGUAGE 'sql';

-- INGREDIENT STORED PROCEDURES

CREATE FUNCTION "spIngredient_GetAll"() RETURNS SETOF "Ingredient" AS 'SELECT * FROM "Ingredient";' LANGUAGE 'sql';

CREATE FUNCTION "spIngredient_GetByName"(ing_name varchar) RETURNS "Ingredient" AS 'SELECT * FROM "Ingredient" WHERE LOWER(ing_name)=LOWER("Name");' LANGUAGE 'sql';

CREATE FUNCTION "spIngredient_InsertValue"(ing_name varchar, price numeric, exp_date timestamp, quantity numeric) RETURNS void AS 'INSERT INTO "Ingredient"("Name", "Price", "Exp_Date", "Quantity") VALUES (ing_name, price, exp_date, quantity);' LANGUAGE 'sql';

CREATE FUNCTION "spIngredient_ModifyByName"(ing_name varchar, price numeric, exp_date timestamp, quantity numeric)RETURNS void AS 'UPDATE "Ingredient" SET "Price" = price, "Exp_Date" = exp_date, "Quantity" = quantity WHERE LOWER(ing_name)=LOWER("Name");' LANGUAGE 'sql';

CREATE FUNCTION "spIngredient_DeleteByName"(ing_name varchar) RETURNS void AS 'DELETE FROM "Ingredient" WHERE LOWER(ing_name)=LOWER("Name");' LANGUAGE 'sql';

-- ORDER STORED PROCEDURES

CREATE FUNCTION "spOrder_GetAll"() RETURNS SETOF "Order" AS 'SELECT * FROM "Order";' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_GetById"(order_id int) RETURNS "Order" AS 'SELECT * FROM "Order" WHERE order_id="Order_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_InsertValue"(user_id int, tran_id int, date_time timestamp) RETURNS void AS 'INSERT INTO "Order"("User_ID", "Transaction_ID", "Date_Time") VALUES (user_id, tran_id, date_time);' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_ModifyById"(order_id int, user_id int, tran_id int, date_time timestamp)RETURNS void AS 'UPDATE "Order" SET "User_ID" = user_id, "Transaction_ID" = tran_id,"Date_Time" = date_time WHERE order_id="Order_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_DeleteById"(order_id int) RETURNS void AS 'DELETE FROM "Order" WHERE order_id="Order_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_GetLastInserted"(out cur_order_id int) AS  $$ SELECT "Order_ID" FROM "Order" ORDER BY "Order_ID" DESC LIMIT 1; $$ LANGUAGE 'sql';

CREATE FUNCTION "spOrder_NumOrdersByTransaction"(in tran_id int, out num_orders bigint) AS  $$ SELECT COUNT("Order_ID") FROM "Order" WHERE tran_id="Transaction_ID"; $$ LANGUAGE 'sql';

CREATE FUNCTION "spOrder_GetCost"(in order_id int, out order_cost numeric) AS  $$ SELECT SUM("Price") FROM "Order_Dish" AS o,"Dish" AS d WHERE o."Dish_ID"=d."Dish_ID" AND o."Order_ID"=order_id; 
$$ LANGUAGE 'sql';

CREATE FUNCTION "spOrder_GetDishes"(order_id int) RETURNS SETOF "Dish" AS $$ SELECT d."Dish_ID",d."Available", d."Price", d."Description", d."Menu_Type" FROM "Dish" AS d, "Order_Dish" AS od WHERE order_id=od."Order_ID" AND d."Dish_ID"=od."Dish_ID"; $$ LANGUAGE 'sql';

-- TRANSACTION STORED PROCEDURES

CREATE FUNCTION "spTransaction_GetAll"() RETURNS SETOF "Transaction" AS 'SELECT * FROM "Transaction";' LANGUAGE 'sql';

CREATE FUNCTION "spTransaction_GetById"(tran_id int) RETURNS "Transaction" AS 'SELECT * FROM "Transaction" WHERE tran_id="Transaction_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spTransaction_InsertValue"(amount numeric, date_time timestamp) RETURNS void AS 'INSERT INTO "Transaction"("Amount", "Date_Time") VALUES (amount, date_time);' LANGUAGE 'sql';

CREATE FUNCTION "spTransaction_ModifyById"(tran_id int, amount numeric, date_time timestamp)RETURNS void AS 'UPDATE "Transaction" SET "Transaction_ID" = tran_id, "Amount" = amount, "Date_Time" = date_time WHERE tran_id="Transaction_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spTransaction_DeleteById"(tran_id int) RETURNS void AS 'DELETE FROM "Transaction" WHERE tran_id="Transaction_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spTransaction_GetLastInserted"(out cur_tran_id int) AS  $$ SELECT "Transaction_ID" FROM "Transaction" ORDER BY "Transaction_ID" DESC LIMIT 1; $$ LANGUAGE 'sql';

CREATE FUNCTION "spTransaction_GetAmount"(in tran_id int, out tran_amount numeric) AS $$SELECT "Amount" FROM "Transaction" WHERE tran_id="Transaction_ID";$$ LANGUAGE 'sql';

CREATE FUNCTION "spTransaction_UpdateAmount"(tran_id int, new_amount numeric) RETURNS void AS  $$UPDATE "Transaction" SET "Amount" = new_amount WHERE tran_id="Transaction_ID";$$ LANGUAGE 'sql';

-- MENU STORED PROCEDURES

CREATE FUNCTION "spMenu_GetAll"() RETURNS SETOF "Menu" AS 'SELECT * FROM "Menu";' LANGUAGE 'sql';

CREATE FUNCTION "spMenu_GetByType"(type varchar) RETURNS "Menu" AS 'SELECT * FROM "Menu" WHERE type="Type";' LANGUAGE 'sql';

CREATE FUNCTION "spMenu_InsertValue"(type varchar, available boolean) RETURNS void AS 'INSERT INTO "Menu"("Type", "Available") VALUES (type, available);' LANGUAGE 'sql';

CREATE FUNCTION "spMenu_ModifyByType"(type varchar, available boolean)RETURNS void AS 'UPDATE "Menu" SET "Available" = available WHERE type="Type";' LANGUAGE 'sql';

CREATE FUNCTION "spMenu_DeleteByType"(type varchar) RETURNS void AS 'DELETE FROM "Menu" WHERE type="Type";' LANGUAGE 'sql';

CREATE FUNCTION "spMenu_GetDishes"(type varchar) RETURNS SETOF "Dish" AS $$ SELECT d."Dish_ID",d."Available", d."Price", d."Description", d."Menu_Type" FROM "Dish" AS d WHERE LOWER(d."Menu_Type")=LOWER(type); $$ LANGUAGE 'sql';

CREATE FUNCTION "spMenu_GetAvailable"() RETURNS SETOF "Menu" AS $$ SELECT * FROM "Menu" WHERE "Available"=true; $$ LANGUAGE 'sql';

-- REVIEW STORED PROCEDURES

CREATE FUNCTION "spReview_GetAll"() RETURNS SETOF "Review" AS 'SELECT * FROM "Review";' LANGUAGE 'sql';

CREATE FUNCTION "spReview_GetById"(user_id integer, review_id integer) RETURNS "Review" AS 'SELECT * FROM "Review" WHERE user_id="User_ID" AND review_id="Review_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spReview_InsertValue"(user_id int, description varchar, rating int, dish_id int) RETURNS void AS 'INSERT INTO "Review"("User_ID", "Description", "Rating", "Dish_ID") VALUES (user_id, description, rating, dish_id);' LANGUAGE 'sql';

CREATE FUNCTION "spReview_ModifyById"( user_id int, review_id int, description varchar, rating int, dish_id int)RETURNS void AS 'UPDATE "Review" SET "Description" = description, "Rating" = rating, "Dish_ID" = dish_id WHERE user_id="User_ID" AND review_id="Review_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spReview_DeleteById"(user_id int, review_id int) RETURNS void AS 'DELETE FROM "Review" WHERE user_id="User_ID" AND review_id="Review_ID";' LANGUAGE 'sql';

-- TABLE STORED PROCEDURES

CREATE FUNCTION "spTable_GetAll"() RETURNS SETOF "Table" AS 'SELECT * FROM "Table";' LANGUAGE 'sql';

CREATE FUNCTION "spTable_GetById"(tableno int) RETURNS "Table" AS 'SELECT * FROM "Table" WHERE tableno="TableNo";' LANGUAGE 'sql';

CREATE FUNCTION "spTable_InsertValue"(location varchar, isoccupied boolean, waiter_id int) RETURNS void AS 'INSERT INTO "Table"("Location", "isOccupied", "waiter_ID") VALUES (location, isoccupied, waiter_id);' LANGUAGE 'sql';

CREATE FUNCTION "spTable_ModifyById"(tableno int, location varchar, isoccupied boolean, waiter_id int)RETURNS void AS 'UPDATE "Table" SET "Location" = location, "isOccupied" = isoccupied, "waiter_ID" = waiter_id WHERE tableno="TableNo";' LANGUAGE 'sql';

CREATE FUNCTION "spTable_DeleteById"(tableno int) RETURNS void AS 'DELETE FROM "Table" WHERE tableno ="TableNo";' LANGUAGE 'sql';

CREATE FUNCTION "spTable_GetWaitedBy"(waiter_id integer) RETURNS SETOF "Table" AS $$ SELECT * FROM "Table" AS t WHERE t."waiter_ID"=waiter_id; $$ LANGUAGE 'sql';

CREATE FUNCTION "spTable_MakeOccupied"(tableno int)RETURNS void AS 'UPDATE "Table" SET "isOccupied" = true WHERE tableno="TableNo";' LANGUAGE 'sql';

CREATE FUNCTION "spTable_MakeDisoccupied"(tableno int)RETURNS void AS 'UPDATE "Table" SET "isOccupied" = false WHERE tableno="TableNo";' LANGUAGE 'sql';

CREATE FUNCTION "spTable_GetOccupied"() RETURNS SETOF "Table" AS $$ SELECT * FROM "Table" WHERE "isOccupied"=true; $$ LANGUAGE 'sql';

-- DISH_INGREDIENT STORED PROCEDURES

CREATE FUNCTION "spDish_Ingredient_GetAll"() RETURNS SETOF "Dish_Ingredient" AS 'SELECT * FROM "Dish_Ingredient";' LANGUAGE 'sql';

CREATE FUNCTION "spDish_Ingredient_GetById"(dish_id int, ing_name varchar) RETURNS "Dish_Ingredient" AS 'SELECT * FROM "Dish_Ingredient" WHERE dish_id="Dish_ID" AND ing_name="Ing_Name";' LANGUAGE 'sql';

CREATE FUNCTION "spDish_Ingredient_InsertValue"(dish_id int, ing_name varchar) RETURNS void AS 'INSERT INTO "Dish_Ingredient"("Dish_ID", "Ing_Name") VALUES (dish_id, ing_name);' LANGUAGE 'sql';

CREATE FUNCTION "spDish_Ingredient_DeleteById"(dish_id int, ing_name varchar) RETURNS void AS 'DELETE FROM "Dish_Ingredient" WHERE dish_id ="Dish_ID" AND ing_name="Ing_Name";' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_Dish_getNumberOfDishes"(in order_id int, out num_dishes bigint) RETURNS bigint AS 'SELECT COUNT(*) FROM "Order_Dish" WHERE order_id="Order_ID"' LANGUAGE 'sql';

-- INGREDIENT_SUPPLIER STORED PROCEDURES

CREATE FUNCTION "spIngredient_Supplier_GetAll"() RETURNS SETOF "Ingredient_Supplier" AS 'SELECT * FROM "Ingredient_Supplier";' LANGUAGE 'sql';

CREATE FUNCTION "spIngredient_Supplier_GetById"(supplier varchar, ing_name varchar) RETURNS "Ingredient_Supplier" AS 'SELECT * FROM "Ingredient_Supplier" WHERE LOWER(supplier)=LOWER("Supplier") AND LOWER(ing_name)=LOWER("Ing_Name");' LANGUAGE 'sql';

CREATE FUNCTION "spIngredient_Supplier_InsertValue"(supplier varchar, ing_name varchar) RETURNS void AS 'INSERT INTO "Ingredient_Supplier"("Supplier", "Ing_Name") VALUES (supplier, ing_name);' LANGUAGE 'sql';

CREATE FUNCTION "spIngredient_Supplier_DeleteById"(supplier varchar, ing_name varchar) RETURNS void AS 'DELETE FROM "Ingredient_Supplier" WHERE LOWER(supplier)=LOWER("Supplier") AND LOWER(ing_name)=LOWER("Ing_Name");' LANGUAGE 'sql';

CREATE FUNCTION "spIngredient_Supplier_getNumberOfSuppliers"(in ing_name varchar, out num_supplier bigint) RETURNS bigint AS 'SELECT COUNT(*) FROM "Ingredient_Supplier" WHERE LOWER(ing_name)=LOWER("Ing_Name");' LANGUAGE 'sql';

CREATE FUNCTION "spIngredient_Supplier_GetIngredientsBySupplier"(supplier varchar) RETURNS TABLE(Ing_Name varchar) AS $$ SELECT isup."Ing_Name" FROM "Ingredient_Supplier" AS isup WHERE LOWER(isup."Supplier")=LOWER(supplier); $$ LANGUAGE 'sql';

-- CUSTOMER_TRANSACTION STORED PROCEDURES

CREATE FUNCTION "spCustomer_Transaction_GetAll"() RETURNS SETOF "Customer_Transaction" AS 'SELECT * FROM "Customer_Transaction";' LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_Transaction_GetById"(user_id int, tran_id int) RETURNS "Customer_Transaction" AS 'SELECT * FROM "Customer_Transaction" WHERE user_id="User_ID" AND tran_id="Transaction_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_Transaction_InsertValue"(user_id int, tran_id int) RETURNS void AS 'INSERT INTO "Customer_Transaction"("User_ID", "Transaction_ID") VALUES (user_id, tran_id);' LANGUAGE 'sql';

CREATE FUNCTION "spCustomer_Transaction_DeleteById"(user_id int, tran_id int) RETURNS void AS 'DELETE FROM "Customer_Transaction" WHERE user_id="User_ID" AND tran_id="Transaction_ID";' LANGUAGE 'sql';

-- ORDER_DISH STORED PROCEDURES

CREATE FUNCTION "spOrder_Dish_GetAll"() RETURNS SETOF "Order_Dish" AS 'SELECT * FROM "Order_Dish";' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_Dish_GetById"(order_id int, dish_id int) RETURNS "Order_Dish" AS 'SELECT * FROM "Order_Dish" WHERE order_id="Order_ID" AND dish_id="Dish_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_Dish_InsertValue"(order_id int, dish_id int) RETURNS void AS 'INSERT INTO "Order_Dish"("Order_ID", "Dish_ID") VALUES (order_id, dish_id);' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_Dish_DeleteById"(order_id int, dish_id int) RETURNS void AS 'DELETE FROM "Order_Dish" WHERE order_id="Order_ID" AND dish_id="Dish_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_Ingredient_NumIngredientInDish"(in dish_id int, out num_ing bigint) RETURNS bigint AS 'SELECT COUNT(*) FROM "Dish_Ingredient" WHERE dish_id="Dish_ID"' LANGUAGE 'sql';

CREATE FUNCTION "spOrder_Dish_GetOrderList"(dish_id int) RETURNS SETOF "Order" AS $$ SELECT "Order_ID", "User_ID", "Transaction_ID", "Date_Time" FROM "Order_Dish" AS od NATURAL JOIN "Order" AS o WHERE od."Dish_ID"=dish_id; $$ LANGUAGE 'sql';

-- ONLINE_ORDER STORED PROCEDURES

CREATE FUNCTION "spOnline_Order_GetAll"() RETURNS SETOF "Online_Order" AS 'SELECT * FROM "Online_Order";' LANGUAGE 'sql';

CREATE FUNCTION "spOnline_Order_GetById"(order_id integer) RETURNS "Online_Order" AS 'SELECT * FROM "Online_Order" WHERE order_id="Order_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spOnline_Order_InsertValue"(order_id int, application varchar) RETURNS void AS 'INSERT INTO "Online_Order"("Order_ID", "Application") VALUES (order_id, application);' LANGUAGE 'sql';

CREATE FUNCTION "spOnline_Order_ModifyById"(order_id int, application varchar)RETURNS void AS 'UPDATE "Online_Order" SET "Application" = application WHERE order_id="Order_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spOnline_Order_DeleteById"(order_id int) RETURNS void AS 'DELETE FROM "Online_Order" WHERE order_id="Order_ID";' LANGUAGE 'sql';

-- IN_STORE_ORDER STORED PROCEDURES

CREATE FUNCTION "spIn_Store_Order_GetAll"() RETURNS SETOF "In_Store_Order" AS 'SELECT * FROM "In_Store_Order";' LANGUAGE 'sql';

CREATE FUNCTION "spIn_Store_Order_GetById"(order_id integer) RETURNS "In_Store_Order" AS 'SELECT * FROM "In_Store_Order" WHERE order_id="Order_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spIn_Store_Order_InsertValue"(order_id int, tableno int) RETURNS void AS 'INSERT INTO "In_Store_Order"("Order_ID", "TableNo") VALUES (order_id, tableno);' LANGUAGE 'sql';

CREATE FUNCTION "spIn_Store_Order_ModifyById"(order_id int, tableno int)RETURNS void AS 'UPDATE "In_Store_Order" SET "TableNo" = tableno WHERE order_id="Order_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spIn_Store_Order_DeleteById"(order_id int) RETURNS void AS 'DELETE FROM "In_Store_Order" WHERE order_id="Order_ID";' LANGUAGE 'sql';

CREATE FUNCTION "spIn_Store_Order_GetOrdersByWaiter"(waiter_id int) RETURNS "In_Store_Order" AS $$ SELECT io."Order_ID", io."TableNo" FROM "In_Store_Order" AS io, "Table" AS t WHERE io."TableNo"=t."TableNo" AND t."waiter_ID"=waiter_id; $$ LANGUAGE 'sql';

CREATE FUNCTION "spIn_Store_Order_GetOrdersByTable"(tableno int) RETURNS "In_Store_Order" AS $$ SELECT io."Order_ID", io."TableNo" FROM "In_Store_Order" AS io WHERE io."TableNo"=tableno; 
$$ LANGUAGE 'sql';
