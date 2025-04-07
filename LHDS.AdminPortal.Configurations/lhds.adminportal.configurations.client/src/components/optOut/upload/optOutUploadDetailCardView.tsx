import { faUpload } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { ChangeEvent, FunctionComponent, useState } from "react";
import ButtonBase from "../../bases/buttons/ButtonBase";
import { OptOut } from "../../../models/optout/optout";

interface OptOutUploadDetailCardViewProps {
    onUpload: (data: OptOut[]) => void;
    onUploadSuccess: boolean;
}

const OptOutUploadDetailCardView: FunctionComponent<OptOutUploadDetailCardViewProps> = (props) => {
    const { onUpload, onUploadSuccess } = props;

    const [file, setFile] = useState<File | null>(null);
    const [nhsNumbers, setNhsNumbers] = useState<string[]>([]);
    const [validNhsNumbers, setValidNhsNumbers] = useState<OptOut[]>([]);
    const [invalidNhsNumbers, setInvalidNhsNumbers] = useState<string[]>([]);

    const splitCSVRows = (csvContent: string): OptOut[] => {
        const rows = csvContent.split('\n');
        const optOuts: OptOut[] = [];

        for (let i = 1; i < rows.length; i++) {
            const row = rows[i].trim();
            if (row === '') {
                continue;
            }

            const columns = row.split(',');

            const optOut: OptOut = {
                id: "",
                nhsNumber: columns[1],
                status: columns[2],
                uniqueReference: columns[0]
            };

            optOuts.push(optOut);
        }

        return optOuts;
    };

    const isValidNhsNumber = (nhsNumber: string): boolean => {
        if (nhsNumber == null || nhsNumber.length !== 10) {
            return false;
        }

        const multipliers = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        let currentNumber = 0;
        let currentSum = 0;
        let currentMultiplier = 0;
        let currentString = "";
        const checkDigit = nhsNumber.substr(nhsNumber.length - 1, 1);
        const checkNumber = parseInt(checkDigit);
        let remainder = 0;
        let total = 0;

        for (let i = 0; i <= 8; i++) {
            currentString = nhsNumber.substr(i, 1);

            currentNumber = parseInt(currentString);
            currentMultiplier = multipliers[i];
            currentSum += currentNumber * currentMultiplier;
        }

        remainder = currentSum % 11;
        total = 11 - remainder;

        if (total === 11) {
            total = 0;
        }

        if (total === checkNumber) {
            return true;
        }

        return false;
    };

    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        const selectedFile = event.target.files?.[0];
        if (selectedFile && selectedFile.type === 'text/csv') {
            setFile(selectedFile);
            setValidNhsNumbers([]);
            setInvalidNhsNumbers([]);

            const reader = new FileReader();

            reader.onload = () => {
                const result = reader.result?.toString();
                const optOuts = splitCSVRows(result || '');

                setNhsNumbers(optOuts.map((optOut) => optOut.nhsNumber));

                const validOptOuts: OptOut[] = [];
                const invalidNhsNumbers: string[] = [];

                optOuts.forEach((optOut) => {
                    if (isValidNhsNumber(optOut.nhsNumber)) {
                        validOptOuts.push(optOut);
                    } else {
                        invalidNhsNumbers.push(optOut.nhsNumber);
                    }
                });
                setValidNhsNumbers(validOptOuts);
                setInvalidNhsNumbers(invalidNhsNumbers);
            };

            reader.readAsText(selectedFile);
        }
    };

    const handleUploadClick = () => {
        if (!file) {
            return;
        }
        onUpload(validNhsNumbers);
    };

    let totalImported = 0;
    let totalInvalid = 0;

    const getSummary = (): string => {
        totalImported = nhsNumbers.length;
        totalInvalid = invalidNhsNumbers.length;
        const totalValid = totalImported - totalInvalid;
        return `${totalValid}/${totalImported} imported. ${totalInvalid} invalid NHS numbers excluded.`;
    };

    return (
        <>
            <div>
                <input type="file" onChange={handleFileChange} /> 

                {!onUploadSuccess && nhsNumbers.length > 0 && (
                    <div>
                        <br />

                        <p style={{ }}><strong>{getSummary()}</strong></p>

                        {totalInvalid > 0 &&
                        <>
                        <h3 style={{ color: "red" }}>Invalid NHS numbers:</h3>
                        <ul>
                            {invalidNhsNumbers.map((nhsNumber, index) => (
                                <li key={index}>
                                    <span style={{ color: "red" }}>{nhsNumber} (Invalid)</span>
                                </li>
                            ))}
                        </ul>

                        <p style={{ color: "red" }}>
                            <strong>NOTE:</strong> Only the valid NHS numbers will be saved.
                        </p>
                        </>
                        }
                        <ButtonBase onClick={handleUploadClick} add>
                            <FontAwesomeIcon icon={faUpload} />
                            &nbsp;Upload VALID NHS Numbers
                        </ButtonBase>
                    </div>
                )}

                {onUploadSuccess && (
                    <div>
                        <h1>POW</h1>
                    </div>
                    )}
            </div>
        </>
    );
};

export default OptOutUploadDetailCardView;