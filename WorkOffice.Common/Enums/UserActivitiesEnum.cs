﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WorkOffice.Common
{
    public enum UserActivitiesEnum : long
    {
        User_Accounts = 1,
        User_Roles = 2,
        Locations = 3,
        Structure_Definition = 4,
        Company_Structures = 5,
        General_Information = 6,
        Custom_Identity_Settings = 7,
        Activity = 8,
        AppType = 9,
        Consultant = 10,
        Hospital = 11,
        Patient_Information = 12,
        Add_Appointment = 13,
        Patient_Appointment = 14,
        View_Booked_Appointment = 15,
        Cancelled_Appointment = 16,
        Patient_Document = 17,
        Add_Waitinglist = 18,
        View_Waitinglist = 19,
        Add_Pathway = 20,
        Validate_now = 21,
    }
}
