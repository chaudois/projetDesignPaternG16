using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAL
{
    public class ContactSQL : SQLBase<ContactDTO>
    {
        public void Add(ContactDTO entity)
        {
            using (SqliteManager sql = new SqliteManager())
            {
                sql.ExecQuery("insert into contact (firstname,lastname,adresse,mail,phonenum) values (" +
                    "'" + entity.firstName + "'" +
                    "'" + entity.lastName + "'" +
                    "'" + entity.adresse + "'" +
                    "'" + entity.mail + "'" +
                    "'" + entity.phoneNum + "');");
            }
        }

        public ContactDTO Get(int id)
        {
            using (SqliteManager sql = new SqliteManager())
            {
                DataTable dt = sql.ExecQuery("select * from contact where id=" + id + ";").Tables[0];
                DataRow dr = dt.Rows[0];
                return new ContactDTO
                {
                    id = int.Parse(dr["id"].ToString()),
                    firstName = dr["firstname"].ToString(),
                    lastName = dr["lastname"].ToString(),
                    mail = dr["mail"].ToString(),
                    adresse = dr["adresse"].ToString(),
                    phoneNum = dr["phoneNum"].ToString()
                };
            }
        }

        public IEnumerable<ContactDTO> GetAll()
        {
            var retour = new List<ContactDTO>();
            using (SqliteManager sql = new SqliteManager())
            {
                DataTable dt = sql.ExecQuery("select * from contact  ;").Tables[0];
                foreach (DataRow row in dt.Rows)
                {

                    retour.Add(new ContactDTO
                    {
                        id = int.Parse(row["id"].ToString()),
                        firstName = row["firstname"].ToString(),
                        lastName = row["lastname"].ToString(),
                        mail = row["mail"].ToString(),
                        adresse = row["adresse"].ToString(),
                        phoneNum = row["phoneNum"].ToString()
                    });

                }
            }
            return retour;
        }

        public void remove(int id)
        {
            throw new NotImplementedException();
        }

        public void update(ContactDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
