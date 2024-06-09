# Questionnaire Generation Toolkit for VR/Non-VR Research


## Running the Demo
1. Download the project.  
2. Open Unity: Simply hit the 'Run' button in Unity to launch the example scene and see the questionnaire toolkit.

## Creating Your Own Questionnaire
### Export Unity Package
1. Export the example scene as a Unity package from the project you downloaded. Make sure to include all dependencies.
2. Open the Unity package you just exported in your own project.

### Setup
1. Drag and drop the Canvas prefab from the prefab folder in the QuestionaireGen folder into your scene to set up the questionnaire interface.
![image](https://github.com/hydu0016/VR-Questionnaire-Gen-Toolkit/assets/95190616/f4cbb8c6-0677-4cdf-a5d0-fe4f28087486)
2. Make sure your scene has an Event System. If it doesn't, you can add one by right-clicking in the Hierarchy, then selecting UI > Event System.
3. Use an existing QuestionnaireConfig Scriptable Object in the QuestionaireGen folder.
![image](https://github.com/hydu0016/VR-Questionnaire-Gen-Toolkit/assets/95190616/b83bd53a-503c-466a-bf1e-b1f12d160f2b)
4. Alternatively, you can create your own `QuestionnaireConfig` Scriptable Object
![image](https://github.com/hydu0016/VR-Questionnaire-Gen-Toolkit/assets/95190616/449a1ff9-c7c1-4506-a6c7-221d7607cb4c)
5. After creating your own QuestionnaireConfig Scriptable Object, drag it to the QuestionnaireGen component attached to the "Canvas" prefab in your scene.
![image](https://github.com/hydu0016/VR-Questionnaire-Gen-Toolkit/assets/95190616/51a68884-5314-44dd-99d3-bc451c6bba8a)

### Configuring Questionnaires
![image](https://github.com/hydu0016/VR-Questionnaire-Gen-Toolkit/assets/95190616/e812b5e2-9a75-49b1-949c-0b5018361297)

1. Select the type of questionnaire you want to create within the `QuestionnaireConfig`. The toolkit supports three types:
   - Likert-scale
   - Single selection
   - Multiple selection
2. Add questions and configure the options for each type:
   - Up to 12 options for Likert-scale questions.
   - Up to 8 options for Single and Multiple selection questions.
3. Save your configurations by pressing `Ctrl+S`.
4. Click the `[Validate Questionnaires]` button to check for any configuration errors.

### Run it
1. If no errors are found, press `Play` in Unity to preview the questionnaire.

## Making it VR 
1. Set the Canvas Render Mode to World Space to ensure it works correctly in a VR environment.
3. You need to enable VR raycasting. Unity's XR Interaction Toolkit provides a good solution for this.
4. If you don't have the XR Interaction Toolkit package, you can add it via the Unity Package Manager. Go to Window > Package Manager, search for XR Interaction Toolkit, and install it.
5. Add the Tracked Device Graphic Raycaster component to your Canvas. This allows the Canvas to interact with XR Ray Interactors.
6. Add the XR UI Input Module component to the Event System:
6. Run your scene and test the interactions. The XR Ray Interactor should allow you to point and interact with the Canvas elements in VR.

