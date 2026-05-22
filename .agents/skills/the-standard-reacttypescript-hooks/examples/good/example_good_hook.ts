// ---
// skill: the-standard-reacttypescript-hooks
// type: example
// source-section: "9. Hooks"
// demonstrates: "tsr-hooks-001, tsr-hooks-004, tsr-hooks-005"
// ---

import { useState, useEffect } from "react";
import { IDashboardViewService } from "../../services/views/dashboard/iDashboardViewService";
import { DashboardView } from "../../models/views/dashboard/DashboardView";

// ✅ Page hook: manages state, calls view service, protects against stale updates
export function useDashboardPage(
    dashboardViewService: IDashboardViewService) {

    const [dashboard, setDashboard] =
        useState<DashboardView | null>(null);

    const [isLoading, setIsLoading] =
        useState<boolean>(true);

    const [error, setError] =
        useState<unknown>(null);

    useEffect(() => {
        // ✅ Mounted flag protects against stale updates
        let isMounted = true;

        async function retrieveDashboardAsync() {
            try {
                const view =
                    await dashboardViewService
                        .retrieveDashboardViewAsync();

                if (isMounted) {
                    setDashboard(view);
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

        retrieveDashboardAsync();

        return () => {
            isMounted = false;
        };
    }, [dashboardViewService]);

    return { dashboard, isLoading, error };
}
