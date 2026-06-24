// ---
// skill: the-standard-reacttypescript-testing
// type: example
// source-section: "16. Testing — foundation service and component"
// demonstrates: "tsr-testing-002, tsr-testing-008"
// ---

import { describe, it, expect, vi } from "vitest";
import { PatientService } from "../../services/foundations/patients/patientService";
import { IPatientApiBroker } from "../../brokers/apis/iPatientApiBroker";

// ✅ Mock only at the broker interface boundary (tsr-testing-008)
const mockBroker: IPatientApiBroker = {
    getPatientAsync: vi.fn()
};

const sut = new PatientService(mockBroker);

describe("PatientService", () => {

    // ✅ Happy path test (tsr-testing-002)
    it("should retrieve patient when id is valid", async () => {
        const expectedPatient = { id: "abc", name: "Elbek" };
        vi.mocked(mockBroker.getPatientAsync).mockResolvedValueOnce(expectedPatient);

        const result = await sut.retrievePatientAsync("abc");

        expect(result).toEqual(expectedPatient);
    });

    // ✅ Validation failure test (tsr-testing-002)
    it("should throw when patient id is empty", async () => {
        await expect(sut.retrievePatientAsync(""))
            .rejects.toThrow();
    });

    // ✅ Exception localization test (tsr-testing-002)
    it("should throw domain exception when broker throws", async () => {
        vi.mocked(mockBroker.getPatientAsync)
            .mockRejectedValueOnce(new Error("Network error"));

        await expect(sut.retrievePatientAsync("abc"))
            .rejects.toThrow("dependency error");
    });
});
