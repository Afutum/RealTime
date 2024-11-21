<html lang="ja">
<head>
    <title>ユーザー登録</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet"
          integrity="sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7SnpJef0486qhLnuZ2cdeRhO02iuK6FUUVM" crossorigin="anonymous">
</head>
<body>
<div class="modal modal-sheet position-static d-block bg-body-secondary p-4 py-md-5" tabindex="-1" role="dialog"
     id="modalSignin">
    <div class="modal-dialog" role="document">
        <div class="modal-content rounded-4 shadow">
            <div class="modal-header p-5 pb-4 border-bottom-0">
                <form method="get" action="{{route('accounts.index')}}">
                    @csrf
                    <h1 class="fw-bold mb-0 fs-2">「{{$account['name']}}」を更新しました</h1>
                    <button type="submit">topへ</button>
                </form>
            </div>
        </div>
    </div>
</div>
