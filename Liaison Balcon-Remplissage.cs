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
					ModelObject SelectedObject = myPicker2.PickObject(0, "Selectionnez le balcon");
					ModelObjectVisualization.ClearAllTemporaryStates();
					Color SeeThroughBlue = new Color(0.0, 0.0, 1.0, 0.5);
					Color SeeThroughRed = new Color(1.0, 0.0, 0.0, 0.5);
					
					List<ModelObject> redObjects = new List<ModelObject>();
					
					if (SelectedObject != null)
					{
						var BlueObjects = new List<ModelObject> { SelectedObject };	
						ModelObjectVisualization.SetTemporaryState(BlueObjects, SeeThroughBlue);	
						SelectedObject.GetUserProperty("PRELIM_MARK", ref PrelimMark);
						if (PrelimMark == "")
						{
							throw new ArgumentNullException("Pas de Repère de Balcon");
						}
						Operation.DisplayPrompt(PrelimMark + " Repère du Balcon");

					}

					
					ModelObjectEnumerator SelectedObjects2 = myPicker2.PickObjects(Picker.PickObjectsEnum.PICK_N_OBJECTS, "Selectionnez les objets puis clic milieu pour valider");
					
					if (SelectedObjects2.GetSize() > 0)
					{
						while (SelectedObjects2.MoveNext())
						{				
							if (SelectedObjects2.Current != null)
							{
								
								redObjects.Add(SelectedObjects2.Current);
								SelectedObjects2.Current.SetUserProperty("PRELIM_MARK", PrelimMark);
							}
						}
						ModelObjectVisualization.SetTemporaryState(redObjects, SeeThroughRed);
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
