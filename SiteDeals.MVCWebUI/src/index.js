import ClassicEditor from './ckeditor';



ClassicEditor
    // Note that you do not have to specify the plugin and toolbar configuration — using defaults from the build.
    .create(document.querySelector('#editor-product-detail'))
    .then(editor => {
        console.log('Editor was initialized', editor);
        window.myEditor = editor;
    })
    .catch(error => {
        console.error(error.stack);
    });