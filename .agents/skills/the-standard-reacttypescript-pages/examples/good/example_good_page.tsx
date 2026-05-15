// ---
// skill: the-standard-reacttypescript-pages
// type: example
// source-section: "8. Pages"
// demonstrates: "tsr-pages-002, tsr-pages-005, tsr-pages-006, tsr-pages-007"
// ---

import { useDashboardPage } from "./useDashboardPage";
import { DashboardComponent } from "../../components/dashboard/DashboardComponent";
import { LoadingIndicator } from "../../components/shared/LoadingIndicator";
import { ErrorSummary } from "../../components/shared/ErrorSummary";
import { EmptyState } from "../../components/shared/EmptyState";

// ✅ Page delegates to hook, handles all states, composes named components
export default function DashboardPage() {

    const {
        dashboard,
        isLoading,
        error
    } = useDashboardPage();

    if (isLoading) {
        return <LoadingIndicator />;
    }

    if (error) {
        return <ErrorSummary error={error} />;
    }

    if (!dashboard) {
        return <EmptyState message="No dashboard data available." />;
    }

    return (
        <DashboardComponent dashboard={dashboard} />
    );
}
