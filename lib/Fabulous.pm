
package Fabulous;
use Moo;
use Function::Parameters;
use Mojo::UserAgent;
use Mojo::URL;

use Fabulous::Article;

has base_url => (is => 'ro', default => 'https://ericlippert.com');
has ua       => (is => 'lazy');

method _build_ua() { 
    return Mojo::UserAgent->new(max_redirects => 5);
}

method get_all_months() {
    return $self->ua->get($self->base_url)->res->dom->find('#archives-2 a')->each;
}

method get_articles_for_month($month) {
    my $month_url = $month->{href};
    my ($yy, $mm) = Mojo::URL->new($month_url)->path =~ /\d+/g;

    my $posts = $self->ua->post(Mojo::URL->new($self->base_url)->query(infinity => 'scrolling') => form => { 
        'action' => "infinite_scroll",
        'page' => "1",
        'order' => "DESC",
        'query_args[year]' => $yy,
        'query_args[monthnum]' => $mm,
        'query_args[posts_per_page]' => 50,
        'query_args[order]' => "DESC",
    });
    my @articles = ();
    for my $article (Mojo::DOM->new($posts->res->json->{html})->find('article')->each) {
        my $title = $article->find('.entry-title > a:nth-child(1)')->first;
        my @tags  = $article->find('span.tag-links > a')->each;
        push @articles, Fabulous::Article->new(title => $title->text, href => $title->{href}, tags => \@tags);
    }
    return @articles;
}

1;

