using Server.UOW;
using Server.Core;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Coupon_Service : Base_Service<Coupon>, ICoupon_Service
    {
        public Coupon_Service
        (
            IUnit_Of_Work_Repo unitOfWork, 
            ICoupon_Repo iRepo
        ) : base(unitOfWork, iRepo)
        {
        }
    }
}
