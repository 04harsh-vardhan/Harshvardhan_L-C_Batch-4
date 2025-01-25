using System.Collections.Generic;
using System.Text;

public class Customer
{
    public int CustomerID { get; set; }
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string Country { get; set; }
}


public class CustomerSearch
{

    // SearchcustomerbyCountry
    public List<Customer> SearchByCountry(string country)
    {

        var query =
    from customer in db.customers
    where
    customer.Country.Contains(country)
    orderby
    customer.CustomerID
    ascending
    select
    customer;


        return query.ToList();

    }

    // Search customer by companyname
public List<Customer> SearchByCompanyName(string company){

        var query =
        from customer in db.customers
        where
        customer.Country.Contains(company)
        orderby
        customer.CustomerID
        ascending
        select
        customer;

        return query.ToList();

    }

    // Search customer by contact person
public List<Customer> SearchByContact(string contact){

        var query =
        from customer in db.customers
        where
        customer.Country.Contains(contact)
        orderby
        customer.CustomerID
        ascending
        select
        customer;

        return query.ToList();

    }

    

}
public class ExportType
{
    public string ExportToCSV(List<Customer> data)
    {

        StringBuilder sb = new StringBuilder();

        foreach (var item in data)
        {
            sb.AppendFormat("{0},{1}, {2}, {3}", item.CustomerID, item.CompanyName, item.ContactName, item.Country);
            sb.AppendLine();
        }

        return sb.ToString();

    }
}