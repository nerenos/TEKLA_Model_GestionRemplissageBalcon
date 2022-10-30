using System;
using System.Collections;
using System.Collections.Generic;

using System.Windows.Forms;
using Tekla.Structures.Model;
using TSG3D = Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.Operations;



namespace Tekla.Technology.Akit.UserScript
{
    public class Script
    {
        public static void Run(Tekla.Technology.Akit.IScript akit)
        {
			string PrelimMark = "";

            try
            {
				while (true)
				{
					Model myModel = new Model();
					myModel.CommitChanges();
					Picker myPicker2 = new Picker();
					ModelObject SelectedObject = myPicker2.PickObject(0, "Selectionnez l'objet");
					ModelObjectVisualization.ClearAllTemporaryStates();
					Color SeeThroughBlue = new Color(0.0, 0.0, 1.0, 0.5);
					Color SeeThroughRed = new Color(0.0, 1.0, 0.0, 0.5);
					
					List<ModelObject> redObjects = new List<ModelObject>();
					
					if (SelectedObject != null)
					{
						SelectedObject.GetUserProperty("PRELIM_MARK", ref PrelimMark);
						if (PrelimMark == "")
						{
							throw new ArgumentNullException("Pas de Repère de Balcon");
						}
						redObjects.Add(SelectedObject);
						Operation.DisplayPrompt(PrelimMark + " Repère du Balcon");
						
						System.Type[] Types = new System.Type[1];
						Types.SetValue(typeof(Part),0);
						ModelObjectEnumerator myEnum = myModel.GetModelObjectSelector().GetAllObjectsWithType(Types);
						
						if (myEnum.GetSize() > 0)
						{
							while (myEnum.MoveNext())
							{				
								if (myEnum.Current != null)
								{
									string PrelimMarkPart = "";
									myEnum.Current.GetUserProperty("PRELIM_MARK", ref PrelimMarkPart);
									if (PrelimMarkPart == PrelimMark)
									{	
										redObjects.Add(myEnum.Current);
									}
								}
							}
							ModelObjectVisualization.SetTemporaryState(redObjects, SeeThroughRed);
						}
						
					}
				}
			}
			catch
            {
	
				if (PrelimMark == "")
				{
				Operation.DisplayPrompt("Pas de Repère de Balcon");
				}else{			
                Operation.DisplayPrompt("Commande interrompue par l'utilisateur");
				}
				ModelObjectVisualization.ClearAllTemporaryStates();
            }
		}
	}
}