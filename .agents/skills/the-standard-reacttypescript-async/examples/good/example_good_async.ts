// ---
// skill: the-standard-reacttypescript-async
// type: example
// source-section: "14. Async and Cancellation"
// demonstrates: "tsr-async-001, tsr-async-002, tsr-async-003, tsr-async-005"
// ---

import { useState, useEffect } from "react";
import { IDashboardViewService } from "../../services/views/dashboard/iDashboardViewService";
import { DashboardView } from "../../models/views/dashboard/DashboardView";

export function useDashboardPage(
    dashboardViewService: IDashboardViewService) {

    // ✅ All four states declared (tsr-async-001)
    const [dashboard, setDashboard] = useState<DashboardView | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [error, setError] = useState<unknown>(null);

    useEffect(() => {
        // ✅ Mounted flag protects against stale updates (tsr-async-002)
        let isMounted = true;

        async function loadAsync() {
            // ✅ try/catch — no ignored rejection (tsr-async-005)
            try {
                // ✅ Promise.all only for independent operations (tsr-async-003)
                const [patients, appointments] = await Promise.all([
                    dashboardViewService.retrievePatientsAsync(),
                    dashboardViewService.retrieveAppointmentsAsync()
                ]);

                if (isMounted) {
                    setDashboard({ patients, appointments });
                }
            } catch (caughtError: unknown) {
                if (isMounted) {
                    setError(caughtError);
                }
            } finally {
                if (isMounted) {
                    setIsLoading(false);
                }
            }
        }

        loadAsync();

        // ✅ Cleanup unmounts the flag (tsr-async-002)
        return () => { isMounted = false; };
    }, [dashboardViewService]);

    return { dashboard, isLoading, error };
}
