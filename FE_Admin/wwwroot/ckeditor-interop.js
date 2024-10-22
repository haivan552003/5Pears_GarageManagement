window.initCKEditor = (elementId, dotnetRef) => {
    CKEDITOR.replace(elementId);
    CKEDITOR.instances[elementId].on('change', function () {
        let data = CKEDITOR.instances[elementId].getData();
        dotnetRef.invokeMethodAsync('OnChange', data);
    });
};