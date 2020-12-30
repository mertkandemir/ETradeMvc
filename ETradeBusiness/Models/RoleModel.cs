using System.Collections.Generic;

namespace ETradeBusiness.Models
{
    public class RoleModel
    {
        #region Entity özellikleri
        public int Id { get; set; }
        
        public string Name { get; set; }
        #endregion

        #region Uygulamada ihtiyacımız olabilecek özellikler
        public List<UserModel> Users { get; set; }
        #endregion
    }
}
