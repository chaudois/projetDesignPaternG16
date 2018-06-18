using System.Collections.Generic;
using System.Data;

using DTO;
namespace DAL
{
    public class ContactSQL : ISQLBase<ContactDTO>
    {
        public void Add(ContactDTO entity)
        {
            SqlSingleton.getInstance().ExecQuery("insert into contact (firstname,lastname) values (" +
            "'" + entity.firstName + "'," +
            "'" + entity.lastName + "');");
            DataTable dt = SqlSingleton.getInstance().ExecQuery(" select id from contact order by id desc limit 1 ;").Tables[0];
            int id = int.Parse(dt.Rows[0]["id"].ToString());
            foreach (var item in entity.fields)
            {
                new FieldSQL().Add(new FieldDTO
                {
                    idContact = id,
                    name = item.name,
                    value = item.value
                });
            }

        }

        public ContactDTO Get(int id)
        {
            DataTable dt = SqlSingleton.getInstance().ExecQuery("select * from contact  where id=" + id + ";").Tables[0];
            DataRow dr = dt.Rows[0];
            ContactDTO retour = new ContactDTO
            {
                id = int.Parse(dr["id"].ToString()),
                firstName = dr["firstname"].ToString(),
                lastName = dr["lastname"].ToString()
            };
            dt = SqlSingleton.getInstance().ExecQuery("select * from field  where idContact=" + id + ";").Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                retour.fields.Add(new FieldDTO
                {
                    idContact = int.Parse(row["idContact"].ToString()),
                    name = row["name"].ToString(),
                    value = row["value"].ToString(),
                });
            }
            return retour;
        }

        public IEnumerable<ContactDTO> GetAll()
        {
            var retour = new List<ContactDTO>();
            List<FieldDTO> fieldOfCurrentReturn = new List<FieldDTO>();

            DataTable dt = SqlSingleton.getInstance().ExecQuery("select * from contact  order by firstname,lastname;").Tables[0];
            for (int i=0;i<dt.Rows.Count;i++)
            {
                retour.Add( new ContactDTO
                {
                    id = int.Parse(dt.Rows[i]["id"].ToString()),
                    firstName = dt.Rows[i]["firstname"].ToString(),
                    lastName = dt.Rows[i]["lastname"].ToString()
                });
                
            }
            foreach (var item in retour)
            {

                dt = SqlSingleton.getInstance().ExecQuery("select * from field  where idContact=" + item.id + ";").Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    item.fields.Add(new FieldDTO
                    {
                        idContact = item.id,
                        name = row["name"].ToString(),
                        value = row["value"].ToString(),
                    });
                }
            }
            return retour;
        }

        public void remove(int id,string name)
        {
            SqlSingleton.getInstance().ExecQuery("delete from field where idContact=" + id + ";");

            SqlSingleton.getInstance().ExecQuery("delete from contact where id=" + id + ";");
        }

        public void update(ContactDTO entity)
        {
            SqlSingleton.getInstance().ExecQuery("update  contact set firstname='" + entity.firstName + "'" +
               ",lastname='" + entity.lastName + "'" +
               " where id=" + entity.id + ";");
            foreach (var item in entity.fields)
            {
                new FieldSQL().update(item);
            }
        }
    }
}
