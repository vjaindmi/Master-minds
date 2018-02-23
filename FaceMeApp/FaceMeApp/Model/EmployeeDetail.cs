using System;
using System.Runtime.Serialization;

namespace FaceMeApp.Model
{
    [DataContract]
    public class EmployeeDetail
    {
        [DataMember(Name = "employeeId")]
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Technology { get; set; }
        public string DeviceId { get; set; }
        public string MACAddress { get; set; }
        public string ManagerEmpId { get; set; }
        public string ManagerEmailId { get; set; }

        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }

        public byte[] Image { get; set; }
    }
}
