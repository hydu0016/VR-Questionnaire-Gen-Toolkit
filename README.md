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
<img width="466" alt="image" src="https://github.com/user-attachments/assets/7794198c-076b-4f95-852f-c2120803064f" />


This toolkit allows you to create and configure questionnaires within Unity and export the data to a CSV file.


**1. Setting Up the CSV Export**
- Specify the file path where the exported CSV file will be saved.


**2. Creating Questions**
You can create different types of questions within `QuestionnaireConfig`. The supported types are:
- **Consent Form** – Requires consent content.
- **Likert Scale** – Allows up to 12 options.
- **Single Selection** – Allows up to 8 options.
- **Multiple Selection** – Allows up to 8 options.
- **Dropdown** – Requires a set of selectable options.
- **Input Field** – Allows users to enter free-text responses.
- **Slider (Left Start)** – Requires left and right labels.
- **Slider (Mid Start)** – Requires left and right labels.



**3. Configuring Questions**
1. Add a question and set its type.
2. Provide a **title/question text** for every question.
3. Configure additional fields based on the question type:
   - **Likert, Single Selection, Multiple Selection, Dropdown**: Add options (ensure they are not empty).
   - **Consent Form**: Provide consent content.
   - **Slider (Left Start & Mid Start)**: Define left and right labels.
4. Ensure that required fields are correctly filled.


**4. Saving and Validating the Configuration**
- Save your configurations by pressing **`Ctrl+S`**.
- Click the **`[Validate]`** button to check for missing or incorrect configurations.
- If validation errors are found, follow the error messages to fix the issues.


**5. Running the Questionnaire**
- Once validation is successful, press **`Play`** in Unity to preview the questionnaire.
- The configured questions will be displayed based on the selected settings.
- Responses will be recorded and stored in the specified CSV file.




## Making it VR 
1. Set the Canvas Render Mode to World Space to ensure it works correctly in a VR environment.
3. You need to enable VR raycasting. Unity's XR Interaction Toolkit provides a good solution for this.
4. If you don't have the XR Interaction Toolkit package, you can add it via the Unity Package Manager. Go to Window > Package Manager, search for XR Interaction Toolkit, and install it.
5. Add the Tracked Device Graphic Raycaster component to your Canvas. This allows the Canvas to interact with XR Ray Interactors.
6. Add the XR UI Input Module component to the Event System:
6. Run your scene and test the interactions. The XR Ray Interactor should allow you to point and interact with the Canvas elements in VR.



## Example:

**Consent Form**  
<img src="https://github.com/user-attachments/assets/0f0d4d28-fb4f-48e4-971d-76aa79dce923" width="500"/>

**Likert Scale**  
<img src="https://github.com/user-attachments/assets/27be75e1-1b3c-48e2-a64a-25eed6f3e7ce" width="500"/>

**Slider (Left Start)**  
<img src="https://github.com/user-attachments/assets/0f7ea646-82f5-4585-8bf5-66c9b5a5becc" width="500"/>

**Slider (Mid Start)**  
<img src="https://github.com/user-attachments/assets/c3ace51d-1364-481b-a514-24322141a4b2" width="500"/>

**Input Field**  
<img src="https://github.com/user-attachments/assets/b41a645d-cc9b-4547-b1fd-87f6882c1591" width="500"/>

**Dropdown**  
<img src="https://github.com/user-attachments/assets/132c63ae-7511-4365-875c-bbe70d81369f" width="500"/>





 
