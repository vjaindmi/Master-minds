using System;
using System.Runtime.Serialization;

namespace FaceMeApp.Model
{
    public class EmployeeDetail
    {
              
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string facePath { get; set; }
        public string EmployeeEmailId { get; set; }
        public string LastName { get; set; }
        public string Technology { get; set; }
        public string DeviceId { get; set; }
        public string MACAddress { get; set; }
        public string ManagerEmpId { get; set; }
        public string ManagerEmailId { get; set; }
        public object InTime { get; set; }
        public object OutTime { get; set; }
        public int CheckInType { get; set; }
        public byte[] ImageContent { get; set; }
       
    }
}