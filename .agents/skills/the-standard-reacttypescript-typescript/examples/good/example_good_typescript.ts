// ---
// skill: the-standard-reacttypescript-typescript
// type: example
// source-section: "2. TypeScript Rules"
// demonstrates: "tsr-typescript-002, tsr-typescript-003, tsr-typescript-004, tsr-typescript-005"
// ---

// ✅ GOOD: Explicit types, type for data, interface for contract, named export, no any.

// Data shape — uses type
export type Patient = {
    id: string;
    name: string;
    age: number;
};

// Behavioral contract — uses interface
export interface IPatientService {
    retrievePatientAsync(patientId: string): Promise<Patient>;
}

// Named export, explicit return type
export async function retrievePatientAsync(
    patientId: string,
    patientApiBroker: IPatientApiBroker)
    : Promise<Patient> {

    return await patientApiBroker.getPatientAsync(patientId);
}

// unknown at unsafe boundary, narrowed to known type
export function parseExternalPatient(raw: unknown): Patient {
    const patient = raw as Patient;

    if (!patient.id || !patient.name) {
        throw new Error("Invalid patient payload.");
    }

    return patient;
}
