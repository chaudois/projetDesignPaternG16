using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data;

namespace DAL
{
    public class FieldSQL : ISQLBase<DTO.FieldDTO>
    {
        public void Add(FieldDTO entity)
        {

            SqlSingleton.getInstance().ExecQuery("insert into field (name,value,idContact) values (" +
            "'" + entity.name + "'," +
            "'" + entity.value + "'," +
            "'" + entity.idContact + "');");
        }

        public FieldDTO Get(int id)
        {
            DataTable dt = SqlSingleton.getInstance().ExecQuery("select * from field where id=" + id + ";").Tables[0];
            DataRow dr = dt.Rows[0];
            return new FieldDTO
            {
                value = dr["value"].ToString(),
                name = dr["name"].ToString(),
                idContact = int.Parse(dr["idContact"].ToString())
            };
        }

        public IEnumerable<FieldDTO> GetAll()
        {
            var retour = new List<FieldDTO>();
            DataTable dt = SqlSingleton.getInstance().ExecQuery("select * from contact  ;").Tables[0];
            foreach (DataRow row in dt.Rows)
            {

                retour.Add(new FieldDTO
                {
                    idContact = int.Parse(row["idContact"].ToString()),
                    name = row["name"].ToString(),
                    value = row["value"].ToString()
                });

            }
            return retour;
        }

        public void remove(FieldDTO field)
        {
            SqlSingleton.getInstance().ExecQuery("delete from field where idcontact ="+ field.idContact + " and name='" + field.name + "';");
        }

        public void update(FieldDTO entity)
        {
            SqlSingleton.getInstance().ExecQuery("update  field set " +
               " value='" + entity.value + "'" +
               " where idContact=" + entity.idContact + "" +
               " and name='" + entity.name + "';");
        }
    }
}
