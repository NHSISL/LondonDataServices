import { faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { FunctionComponent } from "react";
import moment from "moment";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import ButtonBase from "../bases/buttons/ButtonBase";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import GridBase from "../bases/layouts/Grid/GridBase";

interface IngestionTrackingDetailCardViewProps {
    ingestionTracking: IngestionTrackingView;
    onReDecrypt: (ingestionTracking: IngestionTrackingView) => void;
    onRefresh: (ingestionTracking: IngestionTrackingView) => void;
}

const IngestionTrackingDetailCardView: FunctionComponent<IngestionTrackingDetailCardViewProps> = (props) => {
    const {
        ingestionTracking,
        onReDecrypt
    } = props;

    return (
        <>
            <GridBase >
                <CardBase>
                    <CardBaseBody>
                        <CardBaseTitle>Details</CardBaseTitle>
                        <CardBaseContent>
                            <SummaryListBase>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Supplier</SummaryListBaseKey>
                                    <SummaryListBaseValue>{ingestionTracking.supplier?.name}</SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Encrypted File Name</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {ingestionTracking.encryptedFileName}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Decrypted File Name</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {ingestionTracking.decryptedFileName}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Decrypted</SummaryListBaseKey>
                                    <SummaryListBaseValue>{ingestionTracking.decrypted ?
                                        <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                        <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Last Seen</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {moment(ingestionTracking.lastSeen?.toString()).format("Do-MMM-yyyy HH:mm")}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>File Deleted</SummaryListBaseKey>
                                    <SummaryListBaseValue>{ingestionTracking.fileDeleted ?
                                        <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                        <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Record Count</SummaryListBaseKey>
                                    <SummaryListBaseValue>{ingestionTracking.recordCount}</SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Encrypted File Size</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {ingestionTracking.encryptedFileSize}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Decrypted File Size</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {ingestionTracking.decryptedFileSize}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Is Downloading</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        <SummaryListBaseValue>{ingestionTracking.isDownloading ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Is Processing</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        <SummaryListBaseValue>{ingestionTracking.isProcessing ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>RetryCount</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {ingestionTracking.retryCount}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Source Folder Path</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {ingestionTracking.sourceFolderPath}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Last Attempted Date</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {moment(ingestionTracking.lastAttemptedDate?.toString()).format("Do-MMM-yyyy HH:mm")}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>DataSet Specification Id</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {ingestionTracking.dataSetSpecificationId}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Batch</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {ingestionTracking.batch}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Object Name</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {ingestionTracking.objectName}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Batch Ready Folder Path</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {ingestionTracking.batchReadyFolderPath}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Created Date</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {moment(ingestionTracking.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>
                                <SummaryListBaseRow>
                                    <SummaryListBaseKey>Updated Date</SummaryListBaseKey>
                                    <SummaryListBaseValue>
                                        {moment(ingestionTracking.updatedDate?.toString()).format("Do-MMM-yyyy HH:mm")}
                                    </SummaryListBaseValue>
                                </SummaryListBaseRow>

                                {ingestionTracking.supplier?.canDecryptIngestionTracking && ingestionTracking.decrypted && (
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Actions</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            <ButtonBase onClick={() => onReDecrypt(ingestionTracking)} add>
                                                &nbsp;Re-Decrypt Document
                                            </ButtonBase> &nbsp;
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                )}
                            </SummaryListBase>
                        </CardBaseContent>
                    </CardBaseBody>
                </CardBase>
            </GridBase>
        </>
    );
}
export default IngestionTrackingDetailCardView;
