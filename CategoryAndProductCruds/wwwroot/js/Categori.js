
    function deleteCategory(categoryId) {
        $.ajax({
            url: '@Url.Action("Delete", "Category")',
            type: 'POST',
            data: { id: categoryId },
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Deleted!',
                        text: response.message,
                        footer: '<a href="#">Go back to categories</a>'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = '@Url.Action("Index", "Category")';
                        }
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: response.message,
                        footer: '<a href="#">Why do I have this issue?</a>'
                    });
                }
            }
        });
        }
