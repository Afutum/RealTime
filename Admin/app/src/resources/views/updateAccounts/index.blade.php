<html lang="ja">
<head>
    <title>ユーザー登録</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet"
          integrity="sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7SnpJef0486qhLnuZ2cdeRhO02iuK6FUUVM" crossorigin="anonymous">
</head>
<body>
<form method="post" action="{{route('accounts.passUpdate')}}">
    @csrf
    <div class="modal modal-sheet position-static d-block bg-body-secondary p-4 py-md-5" tabindex="-1" role="dialog"
         id="modalSignin">
        <div class="modal-dialog" role="document">
            <div class="modal-content rounded-4 shadow">
                <div class="modal-header p-5 pb-4 border-bottom-0">
                    <h1 class="fw-bold mb-0 fs-2">パスワード更新</h1>
                </div>
                <div class="modal-body p-5 pt-0">
                    <form class="">
                        <div class="form-floating mb-3">
                            <input type="text" class="form-control rounded-3" id="floatingInput" name="name"
                                   value="{{$account['name']}}">
                            <label for="floatingInput"></label>
                        </div>
                        <div class="form-floating mb-3">
                            <input type="password" class="form-control rounded-3" id="floatingPassword"
                                   placeholder="Password" name="password">
                            <label for="floatingPassword">パスワード</label>
                        </div>
                        <div class="form-floating mb-3">
                            <input type="password" class="form-control rounded-3" id="floatingPassword"
                                   placeholder="Password" name="password_confirmation">
                            <label for="floatingPassword">パスワード(再確認)</label>
                        </div>
                        <button class="w-100 mb-2 btn btn-lg rounded-3 btn-primary" type="submit">登録</button>
                        <input type="hidden" name="id" value="{{$account['id']}}">
                    </form>
                    @if(!empty($success))
                        <ul>
                            <li>{{$success}}</li>
                        </ul>
                    @endif
                    @if(!empty($error))
                        <ul>
                            <li class="text-danger">その名前はすでに使われています</li>
                        </ul>
                    @endif
                    @if(!empty($errors))
                            <?php \Barryvdh\Debugbar\Facades\Debugbar::info($errors); ?>
                        @if($errors->any())
                            <ul>
                                @foreach($errors->all() as $error)
                                    <li class="text-danger">{{$error}}</li>
                                @endforeach
                            </ul>
                        @endif
                    @endif
                </div>
            </div>
        </div>
    </div>
</form>
