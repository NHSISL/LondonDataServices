// ---
// skill: the-standard-reacttypescript-styling
// type: example
// source-section: "12. Styling"
// demonstrates: "tsr-styles-001, tsr-styles-002, tsr-styles-005"
// ---

import { PatientCardView } from "../../models/views/patients/PatientCardView";

export type PatientCardProps = {
    patient: PatientCardView;
};

export function PatientCard(props: PatientCardProps) {
    return (
        // ✅ Bootstrap utility classes for layout (tsr-styles-001)
        <div className="card mb-3 p-3">
            <h2 className="card-title">
                {props.patient.displayName}
            </h2>

            {/* ✅ No business rule — statusDisplayText comes from view model (tsr-styles-002) */}
            <p>{props.patient.statusDisplayText}</p>

            {/* ✅ Static class — no domain condition in JSX */}
            <span className="badge bg-secondary">
                {props.patient.ageGroupDisplayText}
            </span>

            {/* ✅ Inline style only for dynamic width from user input (tsr-styles-005) */}
            <div style={{ width: `${props.patient.progressPercent}%` }}
                 className="progress-bar" />
        </div>
    );
}
