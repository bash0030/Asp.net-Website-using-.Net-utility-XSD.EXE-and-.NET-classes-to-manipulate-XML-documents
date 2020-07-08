using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

public partial class RestaurantReview : System.Web.UI.Page
{
    restaurants restaurnts;
    string xmlFile;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        lblConfirmation.Visible = false;
        pnlViewRestaurant.Visible = false;

        xmlFile = MapPath(@"~/App_Data/restaurant_reviews.xml");
        FileStream xs = new FileStream(xmlFile, FileMode.Open);
        XmlSerializer serializor = new XmlSerializer(typeof(restaurants));

        restaurnts = (restaurants)serializor.Deserialize(xs);

        xs.Close();
        

        if (!IsPostBack)
        {
            //Use the names of the restaurants in the XML file  to populate the dropdown list

            drpRestaurants.Items.Insert(0, new ListItem("Select One", "-1"));

            for (int i = 0; i < restaurnts.restaurant.Length; i++)
            {
                ListItem item = new ListItem(restaurnts.restaurant[i].name, i.ToString());

                drpRestaurants.Items.Add(item);

            }

        }
    }

    protected void drpRestaurants_SelectedIndexChanged(object sender, EventArgs e)
    {
        //show the selected restaurant data as specified in the lab requirements 
           pnlViewRestaurant.Visible = true;

        if (drpRestaurants.SelectedValue != "-1")
        {
            string restaurantName = drpRestaurants.SelectedItem.Text;

            for (int i = 0; i < restaurnts.restaurant.Length; i++)
            {
                if (restaurantName == restaurnts.restaurant[i].name.ToString())
                {
                    //address
                    txtAddress.Text = restaurnts.restaurant[i].location.street.ToString();
                    //city
                    txtCity.Text = restaurnts.restaurant[i].location.city.ToString();
                    //province
                    txtProvinceState.Text = restaurnts.restaurant[i].location.provstate.ToString();
                    //postal code
                    txtPostalZipCode.Text = restaurnts.restaurant[i].location.postalzipcode.ToString();
                    //summary
                    txtSummary.Text = restaurnts.restaurant[i].summary.ToString();
                    //rating
                    drpRating.SelectedValue = restaurnts.restaurant[i].rating.ToString();
                }
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        //Save the changed restaurant restaurant data back to the XML file. 
       
        pnlViewRestaurant.Visible = true;
        if (drpRestaurants.SelectedValue != "-1")
        {
            string restaurantName = drpRestaurants.SelectedItem.Text;

            for (int i = 0; i < restaurnts.restaurant.Length; i++)
            {
                if (restaurnts.restaurant[i].name == restaurantName)
                {

                    restaurnts.restaurant[i].summary = txtSummary.Text;
                    restaurnts.restaurant[i].rating = Int32.Parse(drpRating.Text);
                   
                }
            }


            XmlSerializer serializor = new XmlSerializer(typeof(restaurants));
            XmlTextWriter tw = new XmlTextWriter(xmlFile, Encoding.UTF8);

            serializor.Serialize(tw, restaurnts);
            tw.Close();

         // confirmation message
            string confirmMsg = "Revised Resturent Review has been saved to <br>";
            lblConfirmation.Text = confirmMsg + xmlFile;
            lblConfirmation.Visible = true;

        }

    }
}