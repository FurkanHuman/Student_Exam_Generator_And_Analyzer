window.downloadFileFromStream = async (filename, base64Data, mimeType) => {
    const link = document.createElement('a');
    link.download = filename;
    link.href = `data:${mimeType};base64,${base64Data}`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
