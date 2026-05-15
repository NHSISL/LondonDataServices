// ---
// skill: the-standard-reacttypescript-services
// type: example
// source-section: "5. Foundation Services"
// demonstrates: "tsr-foundation-001, tsr-foundation-004, tsr-foundation-005"
// ---

import { Patient } from "../../../models/foundations/patients/Patient";
import { IPatientApiBroker } from "../../../brokers/apis/iPatientApiBroker";
import { validatePatientId } from "./patientService.validations";
import { createFailedPatientRetrievalException } from "./patientService.exceptions";

export interface IPatientService {
    retrievePatientAsync(patientId: string): Promise<Patient>;
}

export class PatientService implements IPatientService {

    public constructor(
        private readonly patientApiBroker: IPatientApiBroker) {
    }

    // ✅ Validates before broker call, localizes exception
    public async retrievePatientAsync(
        patientId: string)
        : Promise<Patient> {

        validatePatientId(patientId);

        try {
            return await this.patientApiBroker
                .getPatientAsync(patientId);
        } catch (error: unknown) {
            throw createFailedPatientRetrievalException(error);
        }
    }
}
