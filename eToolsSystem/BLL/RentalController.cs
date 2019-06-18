using eTools.Data.Entities;
using eTools.Data.POCOs;
using eToolsSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eToolsSystem.BLL
{
    [DataObject]
    public class RentalController
    {
        //List / Search /Get
        public List<RentalEquipment> List_RentalEquipment()
        {
            using (var context = new eToolsContext())
            {
                return context.RentalEquipments.OrderBy(x => x.Description).ToList();
            }
        }

        public List<RentalEquipment> List_RentalEquipmentByDescriptionSearch(string description)
        {
            using (var context = new eToolsContext())
            {
                return context.RentalEquipments.Where(x => x.Description.Contains(description) || x.ModelNumber.Contains(description)).Select(x => x).ToList();
            }
        }

        public List<Customer> List_RentalsByCustomerPhone(string customerPhone)//needs fix i guess? like makes sense I think 
        {
            using (var context = new eToolsContext())
            {
                return context.Customers.Where(x => x.ContactPhone.Contains(customerPhone)).Select(x => x).ToList();//NEED TO FIX LINQ, GET CUSTOMERS FROM RETALS
            }
        }

        public List<Coupon> List_CouponByID(int couponID)
        {
            using (var context = new eToolsContext())
            {
                return context.Coupons.Where(x => x.CouponID.Equals(couponID)).Select(x => x).ToList();
            }
        }

        public int Create_Rental(int employeeID, int customerID, int rentalID)
        {
            //search if rental for same day and employee and customer exists if it does dont let them make one
            //if it does not then create one
            //cancel button will need to delete the rental 
            using (var context = new eToolsContext())
            {
               
                Rental exists = context.Rentals.Where(x => x.CustomerID.Equals(customerID) && x.EmployeeID.Equals(employeeID)).Select(x => x).FirstOrDefault();

                if (exists != null)
                {
                    //dont let them create a thing its already there boi
                    throw new BusinessRuleException("Cannot Have Two Rentals on the Same Day", "One customer and employee pair cannot have two rentals on the same day");
                }
                else
                {
                    exists = new Rental();
                    exists.EmployeeID = employeeID;
                    exists.CustomerID = customerID;

                    exists.PaymentType = "C";
                    exists.SubTotal = 0;
                    exists.TaxAmount = 0;

                    exists = context.Rentals.Add(exists);
                    context.SaveChanges();
                    return exists.RentalID;

                    //select by usering customer employee and date? cause can only return one so if i do linq to get that one then i can get the rental id from that if this is no work.
                }
                
            }
        }


        public void Create_RentalDetails(int rentalID, int equipmentID)
        {
            using (var context = new eToolsContext())
            {

                RentalEquipment equipment = context.RentalEquipments.Where(x => x.RentalEquipmentID.Equals(equipmentID)).Select(x => x).FirstOrDefault();

                RentalDetail exists = context.RentalDetails.Where(x => x.RentalEquipmentID.Equals(equipmentID) && x.RentalID.Equals(rentalID)).Select(x => x).FirstOrDefault();

                if (exists != null)//equipmnet already in there
                {
                    throw new BusinessRuleException("Already Renting This Equipment", "This piece of equipment is already on this rental");
                }
                else
                {
                    RentalDetail detail = new RentalDetail();

                    detail.RentalID = rentalID;
                    detail.RentalEquipment = equipment;
                    detail.ConditionOut = equipment.Condition;
                    detail.ConditionIn = equipment.Condition;

                    context.RentalDetails.Add(detail);

                    context.SaveChanges();
                }
            }
        }
        
        public List<RentalEquipmentInfo> List_RentingEquipment(int rentalID)
        {
            using (var context = new eToolsContext())
            {
                //where rental equipment is in rentaldetails where rentalid == rentalID nested
                var results = from x in context.RentalDetails
                              where x.RentalID.Equals(rentalID)
                              select new RentalEquipmentInfo
                              {
                                  RentalEquipmentID = x.RentalEquipment.RentalEquipmentID,
                                  Description = x.RentalEquipment.Description,
                                  SerialNumber = x.RentalEquipment.SerialNumber,
                                  ModelNumber = x.RentalEquipment.ModelNumber,
                                  DailyRate = x.DailyRate,
                                  Condition = x.ConditionOut
                              };

                return results.ToList();
            }
        }

        public void Cancel_Rental(int rentalID)
        {
            using (var context = new eToolsContext())
            {
                //remove the details then the rental
                var exists = (from x in context.Rentals
                              where x.RentalID.Equals(rentalID)
                              select x).FirstOrDefault();
                if(exists != null)
                {
                    var detailExists = (from x in context.RentalDetails
                                        where x.RentalID.Equals(rentalID)
                                        select x).FirstOrDefault();
                    if(detailExists != null)
                    {
                        //delete all details                        
                        RentalDetail detail = null;
                        List<int> detailsIDs = (from x in context.RentalDetails
                                                where x.RentalID.Equals(rentalID)
                                                select x.RentalDetailID).ToList();
                        foreach (var detailstoDelet in detailsIDs)
                        {
                            detail = exists.RentalDetails.
                                Where(x => x.RentalID == rentalID).
                                FirstOrDefault();
                            if (detail != null)
                            {
                                exists.RentalDetails.Remove(detail);
                            }
                        }
                    }
                    var rentaltoremove = (from x in context.Employees
                                          where x.Rentals.Equals(rentalID)
                                          select x).FirstOrDefault();

                    rentaltoremove.Rentals.Remove(exists);
                }
                context.SaveChanges();
            }
        }

        public List<Rental> Get_RentalsByCusomterSearch(int customerID)
        {
            //need to pick customer then pick rentals from that

            using (var context = new eToolsContext())
            {
                //where rental equipment is in rentaldetails where rentalid == rentalID nested

                var results = from x in context.Rentals
                              where x.CustomerID.Equals(customerID)
                              select new Rental
                              {
                                  RentalID = x.RentalID,
                                  CustomerID = x.CustomerID,
                                  RentalDate = x.RentalDate,
                                  EmployeeID = x.EmployeeID //add more if needed idk
                                  
                              };
                return results.ToList();
            }
        }
        
                                    
        public List<RentalDetail> Get_RentalDetailsByRentalID(int rentalID)
        {
            using (var context = new eToolsContext())
            {
                /*
                    var details = from x in context.RentalDetails
                                             where x.RentalID.Equals(rentalID)
                                             select new RentalDetail
                                             {
                                                 RentalEquipmentID = from y in x.RentalEquipmentID
                                                                   where y.RentalEquipmentID.Equals(x.RentalEquipmentID)
                                                                   select new RentalEquipment {
                                                                       RentalEquipmentID = y.RentalEquipmentID,
                                                                       Description = y.Description,
                                                                       SerialNumber = y.SerialNumber,
                                                                       ModelNumber = y.ModelNumber
                                                                   }
                                             };
                
                    */
                var results = from x in context.RentalDetails
                              where x.RentalID.Equals(rentalID)
                              select new RentalDetail
                              {
                                  //RentalID = x.RentalID,
                                  //RentalDetailID = x.RentalDetailID,
                                  RentalEquipmentID = x.RentalEquipmentID
                                  
                              };

                
                    


                return results.ToList();
            }
        }

        public void updateRental(int rentalID, string creditcard, int couponID)
        {
            using (var context = new eToolsContext())
            {

                Rental rental = context.Rentals.Where(x => x.RentalID.Equals(rentalID)).Select(x => x).FirstOrDefault();


                //RentalDetail exists = context.RentalDetails.Where(x => x.RentalEquipmentID.Equals(equipmentID) && x.RentalID.Equals(rentalID)).Select(x => x).FirstOrDefault();

                if (rental != null)//equipmnet already in there
                {
                    throw new BusinessRuleException("Rental Does not exist", "Rental does not exist, please create a rental before submiting it");
                }
                else
                {
                    Rental update = new Rental();

                    update.RentalID = rentalID;
                    update.CreditCard = creditcard;
                    //update.CustomerID = customerID;
                    //update.EmployeeID = employeeID;
                    update.CouponID = couponID;

                    context.Entry(update).Property(y => y.CreditCard).IsModified = true;
                    context.Entry(update).Property(y => y.CouponID).IsModified = true;
                    
                    context.SaveChanges();
                }
            }
        }


        public void PayNow(int rentalID, string paymentType)
        {
            using (var context = new eToolsContext())
            {
                Rental rental = context.Rentals.Where(x => x.RentalID.Equals(rentalID)).Select(x => x).FirstOrDefault();

                if (rental != null)//equipmnet already in there
                {
                    throw new BusinessRuleException("Rental Does not exist", "Rental does not exist, please create a rental before submiting it");
                }
                else
                {
                    Rental update = new Rental();
                    update.RentalID = rentalID;
                    update.PaymentType = paymentType;
                    context.Entry(update).Property(y => y.PaymentType).IsModified = true;
                    context.SaveChanges();
                }
            }
        }


        public void UpdateRentalTotals(int rentalID)
        {
            using (var context = new eToolsContext())
            {
                RentalDetail detail = new RentalDetail();
                //get the rental details and pull the prices out of it
                Rental update = new Rental();
                List<RentalDetail> rentalDetails = Get_RentalDetailsByRentalID(rentalID);                
            }
        }//method


        public void RentalReturn(int rentalID)
        {
            
        }








    }//class
}
