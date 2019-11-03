using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caroubot
{
    class PostInfo
    {
        short NumberofPhotos { get; set; }
        string Title { get; set; }
        string Brand { get; set; }
        //should be a link to a desciption file.
        string Description { get; set; }
        bool Used { get; set; }
        int Price { get; set; }
        bool MultipleUnits { get; set; }
        bool MeetUp { get; set; }
        bool Delivery { get; set; }
        string Location { get; set; }
        public PostInfo(short numphotos, string title, string brand, string desc, bool used, int price, bool multiple, bool meetup, bool delivery, string location)
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
