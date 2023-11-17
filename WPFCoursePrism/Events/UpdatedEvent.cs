using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFCoursePrism.Models;

namespace WPFCoursePrism.Events
{
    public class UpdatedEvent : PubSubEvent<string>
    {

    }

    public class PersonEvent : PubSubEvent<Person>
    {

    }

}
