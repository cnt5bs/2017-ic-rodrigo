using App.Models;
using ORM.Conteudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Services
{
    public class QuestionarioService
    {
        
        public Questionario InsertQuestionario(Questionario Questionario)=>new QuestionarioDB().InsertQuestionario(Questionario);
        
        public Prototipo InsertProtipo(Prototipo prototipoQuestionario)=>new QuestionarioDB().InsertPrototipo(prototipoQuestionario);
        
        public Questionario findQuestionarioByID(string ID)=>new QuestionarioDB().FindQuestionarioByID(ID);
        
        public Questionario findQuestionarioByuserIDAndPrototype(string prototypeID, string userID)=>new QuestionarioDB().FindQuestionarioByUserIDAndPrototype(userID, prototypeID);
        
        public List<Questionario> ListActiveQuestionarios()=>new QuestionarioDB().ListActiveQuestionarios();
        
        public List<Questionario> ListNotActiveQuestionarios()=>new QuestionarioDB().ListNotActiveQuestionarios();
        
        public void UpdateQuestionarioByID(string ID, Dictionary<string, object> properties)=>new QuestionarioDB().UpdateQuestionarioByID(ID, properties);
        
        public void UpdateQuestionariosByPrototipoID(string prototipoID, Dictionary<string, object> prop_value)
        {
            QuestionarioDB questionarioDB = new QuestionarioDB();
            List<Questionario> prototipos = questionarioDB.ListActiveQuestionariosByPrototipoID(prototipoID);
            foreach(var prototipo in prototipos)
                foreach(var updateProp in prop_value)
                    questionarioDB.UpdateQuestionarioByID(prototipoID, new Dictionary<string, object>() { { updateProp.Key, updateProp.Value } });
        }
        public void DeactivateQuestionariosByID(List<string> IDs)
        {
            foreach (var id in IDs) new QuestionarioDB().UpdateQuestionarioByID(id, new Dictionary<string, object>() { { "active", "false" } });
        }
    }
}