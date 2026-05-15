// ---
// skill: the-standard-reacttypescript-brokers
// type: example
// source-section: "4. Brokers"
// demonstrates: "tsr-brokers-001, tsr-brokers-004, tsr-brokers-005"
// ---

// ✅ GOOD: Interface + thin broker class, one external concern, foundation model return type.

import { Patient } from "../../models/foundations/patients/Patient";

export interface IPatientApiBroker {
    getPatientAsync(patientId: string): Promise<Patient>;
}

export class PatientApiBroker implements IPatientApiBroker {

    public async getPatientAsync(
        patientId: string)
        : Promise<Patient> {

        const response = await fetch(`/api/patients/${patientId}`);

        if (!response.ok) {
            throw new Error(
                `Failed to retrieve patient with id '${patientId}'.`);
        }

        // ✅ mechanical deserialization only — no business rules
        return await response.json() as Patient;
    }
}
