using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;


namespace Mobica
{
    public class Address
    {
        #region members
        public String Country { get; set; }
        public String City { get; set; }
        public String Street { get; set; }
        public String PostCode { get; set; }
        public String Name { get; set; }
        public String Picture { get; set; }
        #endregion

        public Address(String country, String city, String street,
                                String postcode, String name)
        {
            this.Country = country;
            this.City = city;
            this.Street = street;
            this.PostCode = postcode;
            this.Name = name;
            switch (country)
            {
                case "Poland":
                    this.Picture = "Images/pl.png";
                    break;
                case "UK":
                    this.Picture = "Images/gb.png";
                    break;
                case "USA":
                    this.Picture = "Images/us.png";
                    break;
                default:
                    this.Picture = "";
                    break;
            }
        }

        public String GetAddressAsString()
        {
            String adrStr = "";
            if (!String.IsNullOrEmpty(Country))
                adrStr += Country+ " ";
            if (!String.IsNullOrEmpty(City))
                adrStr += City + " ";
            if (!String.IsNullOrEmpty(Street))
                adrStr += Street + " ";
            if (!String.IsNullOrEmpty(PostCode))
                adrStr += PostCode;
            return adrStr;
        }
    }

    static class MobicaAddresses
    {
        readonly
        static public List<Address> mobicaAddressList = new List<Address>();

        //znajduje szczegółowy adres na podstawie nazwy miasta
        static public Address FindDetails(String city)
        {
            Address DetailedAddress = null;
            foreach (Address adr in mobicaAddressList)
            {
                if (String.Equals(city, adr.City))
                {
                    DetailedAddress = adr;
                    break;
                }
            }
            return DetailedAddress;
        }

        static public void AddAddress(Address adr)
        {
            if (!mobicaAddressList.Contains(adr))
                mobicaAddressList.Add(adr);
        }

        static public void RemoveAddress(Address adr)
        {
            if (mobicaAddressList.Contains(adr))
                mobicaAddressList.Remove(adr);
        }


    }
}
