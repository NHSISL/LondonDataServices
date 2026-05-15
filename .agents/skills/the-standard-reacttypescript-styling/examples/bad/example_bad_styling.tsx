// ---
// skill: the-standard-reacttypescript-styling
// type: example
// source-section: "12. Styling"
// demonstrates: "tsr-styles-002, tsr-styles-005"
// ---

// ❌ BAD: Business rule in class, unnecessary inline styles.

import { Patient } from "../../models/foundations/patients/Patient";

export function PatientCard(props: { patient: Patient }) {
    return (
        <div>
            <h2>{props.patient.firstName} {props.patient.lastName}</h2>

            {/* ❌ Business rule in className — violates tsr-styles-002 */}
            <span
                className={props.patient.age >= 18
                    ? "text-success"
                    : "text-danger"}>
                {props.patient.age >= 18 ? "Adult" : "Child"}
            </span>

            {/* ❌ Static inline style instead of utility class — violates tsr-styles-005 */}
            <div style={{ marginTop: "16px", color: "gray", fontSize: "12px" }}>
                Last updated: {props.patient.lastUpdated}
            </div>
        </div>
    );
}
