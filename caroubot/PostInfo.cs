using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caroubot
{
    public class PostInfo
    {
        public int NumberofPhotos { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        //should be a link to a desciption file.
        public string Description { get; set; }
        public bool Used { get; set; }
        public int Price { get; set; }
        public bool MultipleUnits { get; set; }
        public bool MeetUp { get; set; }
        public bool Delivery { get; set; }
        public string Location { get; set; }
        public PostInfo(int numphotos, string title, string brand, string desc, bool used, int price, bool multiple, bool meetup, bool delivery, string location)
        {
            NumberofPhotos = numphotos;
            Title = title;
            Brand = brand;
            Description = desc;
            Used = used;
            Price = price;
            MultipleUnits = multiple;
            MeetUp = meetup;
            Delivery = delivery;
            Location = location;
        }

    }
}
