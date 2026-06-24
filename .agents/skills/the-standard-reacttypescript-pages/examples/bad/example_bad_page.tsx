// ---
// skill: the-standard-reacttypescript-pages
// type: example
// source-section: "8. Pages"
// demonstrates: "tsr-pages-003, tsr-pages-004, tsr-pages-005"
// ---

// ❌ BAD: Broker call, business rule, and missing states in page.

import { useEffect, useState } from "react";

export default function DashboardPage() {

    const [patients, setPatients] = useState<unknown[]>([]);

    // ❌ Direct fetch in page — violates tsr-pages-003
    useEffect(() => {
        fetch("/api/patients")
            .then(response => response.json())
            .then(setPatients);
    }, []);

    // ❌ Business rule in page — violates tsr-pages-004
    const adults = patients.filter((p: any) => p.age >= 18);

    // ❌ No loading, error, or empty states — violates tsr-pages-005
    return <div>{adults.length} adult patients</div>;
}
