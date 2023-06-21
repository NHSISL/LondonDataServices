import { faUpload } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { ChangeEvent, FunctionComponent, useState } from "react";
import ButtonBase from "../../bases/buttons/ButtonBase";

interface OptOutUploadDetailCardViewProps {
    onUpload: (data: string[]) => void;
}

const OptOutUploadDetailCardView: FunctionComponent<OptOutUploadDetailCardViewProps> = (props) => {
    const { onUpload } = props;

    const [file, setFile] = useState<File | null>(null);
    const [nhsNumbers, setNhsNumbers] = useState<string[]>([]);
    const [validNhsNumbers, setValidNhsNumbers] = useState<string[]>([]);
    const [invalidNhsNumbers, setInvalidNhsNumbers] = useState<string[]>([]);

    const splitCSVRows = (csvContent: string) => {
        const rows = csvContent.split('\n');
        const nhsNumbers = rows
            .map((row) => row.replace(/"/g, '').split(',')[0])
            .filter((row) => row.trim() !== '');
        return nhsNumbers;
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
                const nhsNumbers = splitCSVRows(result || '');

                setNhsNumbers(nhsNumbers);

                const validNumbers: string[] = [];
                const invalidNumbers: string[] = [];

                nhsNumbers.forEach((nhsNumber) => {
                    if (isValidNhsNumber(nhsNumber)) {
                        validNumbers.push(nhsNumber);
                    } else {
                        invalidNumbers.push(nhsNumber);
                    }
                });
                setValidNhsNumbers(validNumbers);
                setInvalidNhsNumbers(invalidNumbers);
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

    const getSummary = (): string => {
        const totalImported = nhsNumbers.length;
        const totalInvalid = invalidNhsNumbers.length;
        const totalValid = totalImported - totalInvalid;
        return `${totalValid}/${totalImported} imported. ${totalInvalid} invalid NHS numbers excluded.`;
    };

    return (
        <>
            <div>
                <input type="file" onChange={handleFileChange} /> <br />

                {nhsNumbers.length > 0 && (
                    <div>
                        <br />

                        <p style={{ color: "green" }}><strong>{getSummary()}</strong></p>

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
                        <ButtonBase onClick={handleUploadClick} add>
                            <FontAwesomeIcon icon={faUpload} />
                            &nbsp;Upload VALID NHS Numbers
                        </ButtonBase>
                    </div>
                )}
            </div>
        </>
    );
};

export default OptOutUploadDetailCardView;
