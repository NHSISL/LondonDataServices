// ---
// skill: the-standard-reacttypescript-components
// type: example
// source-section: "7. Components"
// demonstrates: "tsr-components-002, tsr-components-005, tsr-components-007, tsr-components-009"
// ---

import { PatientProfileView } from "../../models/views/patients/PatientProfileView";

// ✅ Typed prop model
export type PatientProfileComponentProps = {
    patientProfile: PatientProfileView;
    onSave: () => void;
};

// ✅ Component receives data through props — no fetch, no service calls
export function PatientProfileComponent(
    props: PatientProfileComponentProps) {

    return (
        <section>
            <h1>{props.patientProfile.displayName}</h1>

            {/* ✅ Display text comes from view model — no business rule in JSX */}
            <p>{props.patientProfile.ageGroupDisplayText}</p>

            {/* ✅ Semantic button element — keyboard accessible */}
            <button type="button" onClick={props.onSave}>
                Save
            </button>
        </section>
    );
}
