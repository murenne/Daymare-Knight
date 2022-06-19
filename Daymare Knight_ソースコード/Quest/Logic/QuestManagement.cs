using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManagement : Singleton<QuestManagement>
{
    [System.Serializable]
    public class  QuestTask
    {
        public QuestData_SO questData;
        public bool isStarted
        { 
            get 
            { 
                return questData.isStarted; 
                
            } 

            set 
            { 
                questData.isStarted = value; 
            }
        }

         public bool isCompleted
        { 
            get 
            { 
                return questData.isCompleted; 
                
            } 
            
            set 
            { 
                questData.isCompleted = value; 
            }
        }

         public bool isFinished
        { 
            get 
            { 
                return questData.isFinished; 
                
            } 
            
            set 
            { 
                questData.isFinished = value; 
            }
        }
    }

    public List<QuestTask> tasks = new List<QuestTask>();

    public bool HaveQuest(QuestData_SO data)
    {
        if(data != null)
        {
            return tasks.Any(q =>q.questData.questName == data.questName) ;
        }
        else
        {
            return false;
        }
    }

    public QuestTask GetTask(QuestData_SO data)
    {
        return tasks.Find(q => q.questData.questName == data.questName);
    }
}
