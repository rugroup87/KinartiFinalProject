using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using WebApplication1.Models;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts a person to the PersonTbl table 
    //--------------------------------------------------------------------------------------------------
    public int Insert(Person person)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("TinderConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        //if(CheckEmailExist(person.Email))
        //{
        //    return -999;
        //}
        //else
        //{
        //}
        String cStr = BuildInsertCommand(person);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }

        catch (SqlException ex)
        {
            if (ex.Number == 2627)
            {
                return -999;
            }
            return 0;
        }

        catch (Exception ex)
        {

            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------

    private String BuildInsertCommand(Person person)
    {
        //int[] temp = new int[person.Hobbies.Length]; 
        String command;
        SqlConnection con;
        con = connect("TinderConnectionString");

        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}' ,'{2}', {3}, {4}, '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}') ", person.Name, person.FamilyName, person.Address, 
            person.Age.ToString(), person.Height.ToString(), person.Gender, person.Phone, person.IsActive.ToString(), person.IsPremium.ToString(), person.Image, person.Email, person.Password);
        String prefix = "INSERT INTO PersonTbl (name, family_name, address, age, height, gender, phone, active, premium, image, email, password) ";
        String prefix2 = "INSERT INTO Hobbies_for_persons (id, Hobbie_id) ";
        command = prefix + sb.ToString() + prefix2 + "Values";
        SqlCommand cmd = new SqlCommand("SELECT TOP 1 * from PersonTbl order by id desc", con);
        for (int i = 0; i < person.Hobbies.Length; i++)
        {
            sb2.AppendFormat("({0}, {1})", Convert.ToInt32(cmd.ExecuteScalar()) + 1, person.Hobbies[i]+1);
            if (i < (person.Hobbies.Length-1))
            {
                sb2.Append(",");
            }      
        }
        command += sb2.ToString();

        return command;
    }


    //public bool CheckEmailExist(string email)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    SqlConnection con = connect("TinderConnectionString");
    //    sb.AppendFormat("SELECT * from PersonTbl where email = '{0}'", email);
    //    SqlCommand cmd = new SqlCommand(sb.ToString(), con);
    //    if(Convert.ToInt32(cmd.ExecuteScalar()) > 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------

    public List<Person> GetAllPersons()
    {

        SqlConnection con;
        List<Person> lp = new List<Person>();

        try
        {

            con = connect("TinderConnectionString"); // create a connection to the database using the connection String defined in the web config file
        }

        catch (Exception ex)
        {
            // write to log
            throw (ex);

        }

        try
        {
            String selectSTR = "SELECT * FROM PersonTbl";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            //int PID = Convert.ToInt32(cmd.ExecuteScalar());
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read())
            {// Read till the end of the data into a row
                // read first field from the row into the list collection
               
                Person p = new Person();
                p.ID = Convert.ToInt32(dr["id"]);
                p.Email = Convert.ToString(dr["email"]);
                p.Name = Convert.ToString(dr["name"]);
                p.FamilyName = Convert.ToString(dr["family_name"]);
                p.Address = Convert.ToString(dr["address"]);
                p.Gender = Convert.ToString(dr["gender"]);
                p.Age = Convert.ToSingle(dr["age"]);
                p.Height = Convert.ToSingle(dr["height"]);
                if (!DBNull.Value.Equals(dr["active"]))
                {
                    p.IsActive = Convert.ToBoolean(dr["active"]);
                }
                if (!DBNull.Value.Equals(dr["premium"]))
                {
                    p.IsPremium = Convert.ToBoolean(dr["premium"]);
                }
                if (!DBNull.Value.Equals(dr["phone"]))
                {
                    p.Phone = Convert.ToString(dr["phone"]);
                }
                p.Image = Convert.ToString(dr["image"]);
                p.Hobbies = GetPersonHobbiesArr(p.ID);
                lp.Add(p);
            }
            return lp;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);

        }

    }

    public int[] GetPersonHobbiesArr(int index)
    {

        SqlConnection con;
        List<int> HobbieArr = new List<int>();

        try
        {
            con = connect("TinderConnectionString"); // create a connection to the database using the connection String defined in the web config file
        }

        catch (Exception ex)
        {
            // write to log
            throw (ex);

        }

        try
        {
            String selectSTR = "SELECT * FROM Hobbies_for_persons where id = '"+index+"'";

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read())
            {// Read till the end of the data into a row
                // read first field from the row into the list collection
                HobbieArr.Add(Convert.ToInt32(dr["Hobbie_Id"]));
            }
            return HobbieArr.ToArray();
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);

        }

    }

    public List<Hobbies> ReadHobbies()
    {

        SqlConnection con;
        List<Hobbies> LH = new List<Hobbies>();

        try
        {

            con = connect("TinderConnectionString"); // create a connection to the database using the connection String defined in the web config file
        }

        catch (Exception ex)
        {
            // write to log
            throw (ex);

        }

        try
        {
            String selectSTR = "SELECT * FROM Hobbies";

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read())
            {// Read till the end of the data into a row
                // read first field from the row into the list collection
                Hobbies Hob = new Hobbies();
                Hob.Hname = Convert.ToString(dr["Hobbie_name"]);
                LH.Add(Hob);
            }
            return LH;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);

        }
    }

    //public List<Person> Filter(Filter f)
    //{

    //    SqlConnection con;
    //    List<Person> lp = new List<Person>();

    //    try
    //    {

    //        con = connect("TinderConnectionString"); // create a connection to the database using the connection String defined in the web config file
    //    }

    //    catch (Exception ex) { 
    //    // write to log
    //        throw (ex);

    //    }

    //    try
    //    {
    //        String selectSTR = "SELECT * FROM PersonTbl";

    //        SqlCommand cmd = new SqlCommand(selectSTR, con);

    //        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
    //        float age;
    //        string gender;
    //        while (dr.Read())
    //        {// Read till the end of the data into a row
    //            // read first field from the row into the list collection
    //            age=Convert.ToSingle(dr["age"]);
    //            gender = Convert.ToString(dr["gender"]);                
    //            if (age<=f.MaxAge && age>=f.MinAge && gender == f.Gender)
    //            {
    //                Person p = new Person();
    //                p.Name=Convert.ToString(dr["name"]);
    //                p.FamilyName = Convert.ToString(dr["family_name"]);
    //                p.Address = Convert.ToString(dr["address"]);
    //                p.Gender = Convert.ToString(dr["gender"]);
    //                p.Age = Convert.ToSingle(dr["age"]);
    //                p.Height = Convert.ToSingle(dr["height"]);
    //                p.Image = Convert.ToString(dr["image"]);
    //                //if (gender == "Male")   p.Image = "<img src=Tinder/Images/male.jpg />";
    //                //else p.Image = "<img src=Tinder/Images/female.jpg />";
    //                lp.Add(p);
    //            }                
    //        }
    //        return lp;
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);

    //    }
    //}


    ////--------------------------------------------------------------------
    //// Read from the DB into a table
    ////--------------------------------------------------------------------
    //public void readCarsDataBase()
    //{

    //    SqlConnection con = connect("carsConnectionString"); // open the connection to the database/

    //    String selectStr = "SELECT * FROM Cars"; // create the select that will be used by the adapter to select data from the DB

    //    SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

    //    DataSet ds = new DataSet("carsDS"); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB

    //    da.Fill(ds);       // Fill the datatable (in the dataset), using the Select command

    //    dt = ds.Tables[0]; // point to the cars table , which is the only table in this case
    //}

    public void ActivityChange(int activity, int PersonId)
    {
        SqlConnection con = connect("TinderConnectionString"); // open the connection to the database/
        
        String selectStr = String.Format("SELECT * FROM PersonTbl where id = {0}", PersonId); // create the select that will be used by the adapter to select data from the DB
        
        SqlDataAdapter da = new SqlDataAdapter(selectStr, con); // create the data adapter

        SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);
        
        DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
        
        da.Fill(ds, "Persons");       // Fill the datatable (in the dataset), using the Select command
        
        //dt = ds.Tables[0]; // point to the cars table , which is the only table in this case

        //dt.Rows[PersonId]["active"] = activity;
        ds.Tables["Persons"].Rows[0]["active"] = activity;
        da.Update(ds,"Persons");
        con.Close();

    }
    
    public string UserValidation(string email, string password)
    {
        string returnedID = "NoUser";
        SqlConnection con;

        try
        {
            con = connect("TinderConnectionString"); // create a connection to the database using the connection String defined in the web config file
        }

        catch (Exception ex)
        {
            // write to log
            throw (ex);

        }

        try
        {
            String selectSTR = "SELECT * FROM PersonTbl where email='"+ email +"' and password='"+ password +"'";

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read())
            {// Read till the end of the data into a row
                // read first field from the row into the list collection
                returnedID = Convert.ToString(dr["email"]);
            }
            return returnedID;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);

        }
    }

    public Person GetUserDetailsByEmail(string email)
    {
        Person p = new Person();
        SqlConnection con;

        try
        {
            con = connect("TinderConnectionString"); // create a connection to the database using the connection String defined in the web config file
        }

        catch (Exception ex)
        {
            // write to log
            throw (ex);

        }

        try
        {
            String selectSTR = "SELECT * FROM PersonTbl where email='" + email + "'";

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read())
            {// Read till the end of the data into a row
             // read first field from the row into the list collection
                
                p.ID = Convert.ToInt32(dr["id"]);
                p.Name = Convert.ToString(dr["name"]);
                p.FamilyName = Convert.ToString(dr["family_name"]);
                p.Address = Convert.ToString(dr["address"]);
                p.Gender = Convert.ToString(dr["gender"]);
                p.Age = Convert.ToSingle(dr["age"]);
                p.Height = Convert.ToSingle(dr["height"]);
                p.Password = Convert.ToString(dr["password"]);
                p.Email = Convert.ToString(dr["email"]);
                if (!DBNull.Value.Equals(dr["active"]))
                {
                    p.IsActive = Convert.ToBoolean(dr["active"]);
                }
                if (!DBNull.Value.Equals(dr["premium"]))
                {
                    p.IsPremium = Convert.ToBoolean(dr["premium"]);
                }
                if (!DBNull.Value.Equals(dr["phone"]))
                {
                    p.Phone = Convert.ToString(dr["phone"]);
                }
                p.Image = Convert.ToString(dr["image"]);
                p.Hobbies = GetPersonHobbiesArr(p.ID);
            }
            return p;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);

        }
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public int UpdatePerson(Person person)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("TinderConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildUpdateCommand(person);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    //------------------------------------------------------------------------------------------------
    public string BuildUpdateCommand(Person person)
    {
        //int[] temp = new int[person.Hobbies.Length];
        String command;
        SqlConnection con;
        con = connect("TinderConnectionString");

        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        StringBuilder sb3 = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("UPDATE PersonTbl SET name = '{0}', family_name = '{1}', address = '{2}', age = '{3}', height = '{4}', gender = '{5}', phone = '{6}', active = '{7}', premium = '{8}', image = '{9}', email = '{10}', password = '{11}' where email = '{12}'", person.Name, person.FamilyName, person.Address,
            person.Age.ToString(), person.Height.ToString(), person.Gender, person.Phone, person.IsActive.ToString(), person.IsPremium.ToString(), person.Image, person.Email, person.Password, person.Email);
        //SqlCommand cmd = new SqlCommand("SELECT * from PersonTbl where email = {}", con);
        sb2.AppendFormat("DELETE FROM Hobbies_for_persons WHERE id = {0} ", person.ID) ;
        String prefix = "INSERT INTO Hobbies_for_persons (id, Hobbie_id) ";
        command = sb.ToString() + sb2.ToString() + prefix + "Values";
        for (int i = 0; i < person.Hobbies.Length; i++)
        {
            sb3.AppendFormat("({0}, {1})", person.ID, person.Hobbies[i] + 1);
            if (i < (person.Hobbies.Length - 1))
            {
                sb3.Append(",");
            }
        }
        command += sb3.ToString();

        return command;
    }
    //-------------------------------------------------------------------------------------------------------------
    ////--------------------------------------------------------------------
    //// insert a movie
    ////--------------------------------------------------------------------
    //public int delete(string prefix)
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("moviesDBConnectionString"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    String cStr = BuildDeleteCommand(prefix);      // helper method to build the insert string

    //    cmd = CreateCommand(cStr, con);             // create the command

    //    try
    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }
    //    catch (Exception ex)
    //    {
    //        return 0;
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)
    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}

    //private string BuildDeleteCommand(string prefix) { 


    //    string cmdStr = "DELETE FROM Movies WHERE actor LIKE '" + prefix + "%'";
    //    return cmdStr;


    //}

}
