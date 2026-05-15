// ---
// skill: the-standard-reacttypescript-testing
// type: example
// source-section: "16. Testing — component visible behavior"
// demonstrates: "tsr-testing-004"
// ---

import { render, screen } from "@testing-library/react";
import { describe, it, expect } from "vitest";
import { PatientProfileComponent } from "../../components/patients/PatientProfileComponent";
import { PatientProfileView } from "../../models/views/patients/PatientProfileView";

const testProfile: PatientProfileView = {
    id: "abc",
    displayName: "Elbek Alimov",
    ageGroupDisplayText: "Adult"
};

describe("PatientProfileComponent", () => {

    it("should render patient display name", () => {
        render(<PatientProfileComponent patientProfile={testProfile} />);

        // ✅ Queries by visible text — not by className or state (tsr-testing-004)
        expect(screen.getByRole("heading", { name: "Elbek Alimov" }))
            .toBeInTheDocument();
    });

    it("should render age group display text", () => {
        render(<PatientProfileComponent patientProfile={testProfile} />);

        expect(screen.getByText("Adult")).toBeInTheDocument();
    });
});
