# Questionnaire Generation Toolkit for VR/Non-VR Research

## Getting Started

### Installation
1. Download the standalone `unitypackage` for Unity version 2022.3.17f1.
2. Import the package into your Unity project.
3. Open the example scene provided within the package to explore a demo setup.

### Running the Demo
- Simply hit the 'Run' button in Unity to launch the demo and see the questionnaire toolkit in action.

## Creating Your Own Questionnaire

### Setup
1. Drag and drop the `Canvas` prefab from the toolkit into your scene to start setting up the questionnaire interface.
2. Use an existing `QuestionnaireConfig` Scriptable Object from the package or create a new one for your questionnaire configuration.

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

### Validation and Preview
1. Click the `[Validate Questionnaires]` button to check for any configuration errors.
2. If no errors are found, press `Play` in Unity to preview the questionnaire.


